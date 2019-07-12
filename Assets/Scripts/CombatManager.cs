using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShrinelandsTactics;
using UnityEngine.Tilemaps;
using YamlDotNet.RepresentationModel;
using System.IO;
using System.Text;
using Assets.Scripts.FSM;
using Assets.Scripts.Events;
using System;
using Assets.Scripts.UI;
using ShrinelandsTactics.World;
using UnityEngine.EventSystems;
using System.Linq;
using ShrinelandsTactics.BasicStructures;
using Assets.Scripts.Networking;

namespace Assets.Scripts
{
    public class CombatManager : MonoBehaviour
    {
        public DungeonMaster DM;
        public Canvas canvas;
        public Tilemap tileMap;
        public Tilemap overlayMap;
        public UnityEngine.Tilemaps.Tile overlayTile;
        public NameplateUI Nameplate;
        public AbilityPanelUI AbilityPanel;
        public CharacterPopupUI PopupPanel;

        public UnityEngine.Tilemaps.Tile emptyTile;
        public UnityEngine.Tilemaps.Tile wallTile;


        public GameObject characterPrefab;
        public GameObject DeckPrefab;

        public TextAsset characterJson;
        public TextAsset tileJson;
        public TextAsset actionJson;
        public TextAsset levelYaml;
        public Texture2D levelLayout;
        private GameData data;

        public bool Connected = false;
        public List<Guid> PlayingAs = new List<Guid>();
        public float CamVelocity;
        public float CamMaxZoom;
        public float CamZoomSpeed;

        private Camera camera;
        public NetworkManager nm;
        public Animator anim { get; private set; }
        public Character SelectedCharacter { get; internal set; }
        public Character MouseoverCharacter { get; internal set; }
        public ShrinelandsTactics.Mechanics.Action SelectedAction { get; internal set; } 

        public event CharacterClickedEventHandler CharacterClicked;
        public event EventHandler<Vector3> TileClicked;
        public event EventHandler<Vector3> OverlayClicked;
        public event EventHandler Deselect;

        void Start()
        {
            camera = Camera.main;
            anim = GetComponent<Animator>();
            MouseoverCharacter = null;

            data = GameData.CreateFromJson(tileJson.text, characterJson.text, actionJson.text);

           
        }

        public void Setup(NetworkManager nm, DungeonMaster DM)
        {
            this.DM = DM;
            this.nm = nm;
            DM.OnCharacterMoved += OnCharacterMoved;
            DM.OnCharacterCreated += OnCharacterCreated;
            DM.OnCardDrawn += OnCardDrawn;
            DM.OnTurnPassed += OnTurnPassed;
            DM.OnStatChanged += StatChanged;
            SetupTiles();
            SetupUnits();
            Connected = true;
        }

        private void StatChanged(object sender, ShrinelandsTactics.BasicStructures.Events.StatChangedEventArgs a)
        {
            var guy = DM.Characters.First(c => c.ID == a.CharacterID);
            Nameplate.UpdateCharacter(guy);
        }

        private void OnTurnPassed(object sender, Guid e)
        {
            //TODO: pop up turn number and side info on UI
        }

        private void OnCardDrawn(object sender, ShrinelandsTactics.BasicStructures.Events.CardDrawnEventArgs a)
        {
            var go = Instantiate(DeckPrefab, canvas.transform);
            var dr = go.GetComponent<DeckRenderer>();
            dr.RenderDeck(a.deck);
            StartCoroutine(dr.DrawCard(a.card.Name));
        }

        public void LoadEncounter()
        {
            throw new NotImplementedException(); //TODO: doesn't connect right stuff
            var yaml = new YamlStream();
            using (StringReader r = new StringReader(levelYaml.text))
            {
                yaml.Load(r);
            }

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var mapFile = (mapping[new YamlScalarNode("map_file")] as YamlScalarNode).Value;


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Test Map");
            sb.AppendLine(levelLayout.width + " " + levelLayout.height);

            for (int y = 1; y <= levelLayout.height; y++)
            {
                for (int x = 0; x < levelLayout.width; x++)
                {
                    var color = levelLayout.GetPixel(x, levelLayout.height - y);
                    var r = (byte)(color.r * 255);
                    var g = (byte)(color.g * 255);
                    var b = (byte)(color.b * 255);
                    sb.Append(data.GetIconByColor(r, g, b).ToString());
                }
                sb.AppendLine();
            }

            DM = DungeonMaster.LoadEncounter(mapping, sb.ToString(), data);
        }

        private void OnCharacterCreated(object sender, Character e)
        {
            SetupUnits();
        }

        private void OnCharacterMoved(object sender, ShrinelandsTactics.BasicStructures.Events.CharacterMovedEventArgs a)
        {
            var renderer = GetComponentsInChildren<CharacterRenderer>().First(cr => cr.CharacterRepresented.ID == a.ID);
            renderer.UpdatePosition(this);
        }

        public void StartTargetingAction(Character guy, string actionName)
        {
            Debug.Log(guy.Name + " is choosing target for " + actionName);
            SelectedAction = guy.Actions.First(a => a.Name.Equals(actionName));
            anim.SetTrigger("Targeting");
        }

        public void PutOverlayTilesAt(List<Vector3Int> places)
        {
            overlayMap.ClearAllTiles();
            foreach (var pos in places)
            {
                overlayMap.SetTile(pos, overlayTile);
            }
        }

        public void SetupTiles()
        {
            overlayMap.ClearAllTiles();
            tileMap.ClearAllTiles();
            for (int y = 0; y < DM.map.Height; y++)
            {
                for (int x = 0; x < DM.map.Width; x++)
                {
                    var tile = DM.map.GetTile(x, y);
                    if (tile.Passable)
                    {
                        tileMap.SetTile(new Vector3Int(x, DM.map.Height-y, 0), emptyTile);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(x, DM.map.Height-y, 0), wallTile);
                    }
                }
            }
        }

        public void SetupUnits()
        {
            foreach (Transform child in transform)
            {
                if(child.gameObject.CompareTag("Character"))
                {
                    Destroy(child.gameObject);
                }
            }
            foreach (var guy in DM.Characters)
            {
                var pos = new Vector3(guy.Pos.x, DM.map.Height - guy.Pos.y + 1, 0);
                var go = Instantiate(characterPrefab, pos, Quaternion.identity, this.transform);
                var controller = go.GetComponent<CharacterRenderer>();
                controller.RepresentCharacter(guy);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(!Connected)
            {
                return;
            }
            //handle input
            //move camera
            //TODO: hold middle mouse button to pan around
            var cameraX = camera.transform.position.x + CamVelocity * Input.GetAxis("Horizontal") * Time.deltaTime;
            var cameraY = camera.transform.position.y + CamVelocity * Input.GetAxis("Vertical") * Time.deltaTime;
            camera.transform.position = new Vector3(cameraX, cameraY, camera.transform.position.z);
            camera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * CamZoomSpeed * Time.deltaTime;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 2, CamMaxZoom);

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(Deselect != null)
                {
                    Deselect(this, null);
                }
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            //check for mouseover events
            if(hits != null && hits.Any(h => h.collider.tag == "Character"))
            {
                var hit = hits.First(h => h.collider.tag == "Character");
                var guy = hit.collider.GetComponent<CharacterRenderer>().CharacterRepresented;
                if(guy != MouseoverCharacter)
                {
                    PopupPanel.ShowCharacter(guy);
                    MouseoverCharacter = guy;
                }
            }
            else
            {
                if(MouseoverCharacter != null)
                {
                    PopupPanel.Clear();
                    MouseoverCharacter = null;
                }
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                //find out what we clicked on
                //is it unit?
                if(hits.Any(h => h.collider.tag == "Overlay"))
                {
                    if(OverlayClicked != null)
                    {
                        Debug.Log("Clicked on overlay at " + mousePos);
                        OverlayClicked(this, mousePos);
                    }
                }
                else if (hits.Any(h => h.collider.tag == "Character"))
                {
                    var hit = hits.First(h => h.collider.tag == "Character");
                    Debug.Log(hit.collider.name);
                    var guy = hit.collider.GetComponent<CharacterRenderer>().CharacterRepresented;
                    if (CharacterClicked != null)
                    {
                        CharacterClicked(this, new CharacterClickedEventArgs(guy));
                    }
                }
                else if (hits.Any(h => h.collider.tag == "Tilemap"))
                {
                    if (TileClicked != null)
                    {
                        Debug.Log("Clicked on tile at " + mousePos);
                        TileClicked(this, mousePos);
                    }
                }

             
            }
        }

        public void EndTurn()
        {
            //TODO: check if legal
            var outcome = DM.EndTurn(DM.currentSideID);
            nm.SendAction(outcome);
            anim.SetTrigger("EndTurn");
        }

        public void Activate(Character guy)
        {
            var outcome = DM.Activate(guy); //TODO: check for errors
            nm.SendAction(outcome);
            anim.SetTrigger("Activate");
        }

        public void Deactivate(Character guy)
        {
            var outcome = DM.Deactivate(guy); //TODO: check for errors
            nm.SendAction(outcome);
            anim.SetTrigger("Deactivate");
        }

        public ShrinelandsTactics.BasicStructures.Position UnityToShrinelandsPosition(Vector3 x)
        {
            Vector3Int rounded = new Vector3Int(Mathf.FloorToInt(x.x), Mathf.FloorToInt(x.y), Mathf.FloorToInt(x.z));
            return new ShrinelandsTactics.BasicStructures.Position(rounded.x, DM.map.Height - rounded.y);
        }

        internal Vector3Int ShrinelandsToUnityVector(Position pos)
        {
            return new Vector3Int(pos.x, DM.map.Height - pos.y, 0);
        }

        public delegate void CharacterClickedEventHandler(object sender, CharacterClickedEventArgs a);
    }

}
