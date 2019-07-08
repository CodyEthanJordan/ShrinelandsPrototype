using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShrinelandsTactics;
using UnityEngine.Tilemaps;
using YamlDotNet.RepresentationModel;
using System.IO;
using System.Text;

namespace Assets.Scripts
{
    public class CombatManager : MonoBehaviour
    {
        public DungeonMaster DM;
        public Tilemap tileMap;
        public Tile emptyTile;
        public Tile wallTile;
        public GameObject characterPrefab;

        public TextAsset characterJson;
        public TextAsset tileJson;
        public TextAsset actionJson;
        public TextAsset levelYaml;
        public Texture2D levelLayout;

        public float CamVelocity;
        public float CamMaxZoom;
        public float CamZoomSpeed;

        private Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            camera = Camera.main;

            var data = GameData.CreateFromJson(tileJson.text, characterJson.text, actionJson.text);

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

            for (int y = 0; y < levelLayout.height; y++)
            {
                for (int x = 0; x < levelLayout.width; x++)
                {
                    var color = levelLayout.GetPixel(x, y);
                    var r = (byte)(color.r * 255);
                    var g = (byte)(color.g * 255);
                    var b = (byte)(color.b * 255);
                    sb.Append(data.GetIconByColor(r, g, b).ToString());
                }
                sb.AppendLine();
            }

            DM = DungeonMaster.LoadEncounter(mapping, sb.ToString(), data);
            SetupTiles();
            SetupUnits();
        }

        public void SetupTiles()
        {
            tileMap.ClearAllTiles();
            for (int y = 0; y < DM.map.Height; y++)
            {
                for (int x = 0; x < DM.map.Width; x++)
                {
                    var tile = DM.map.GetTile(x, y);
                    if (tile.Passable)
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(x, y, 0), wallTile);
                    }
                }
            }
        }

        public void SetupUnits()
        {
            foreach (var guy in DM.Characters)
            {
                var pos = new Vector3(guy.Pos.x, DM.map.Height - guy.Pos.y, 0);
                var go = Instantiate(characterPrefab, pos, Quaternion.identity, this.transform);
                var controller = go.GetComponent<CharacterRenderer>();
                controller.RepresentCharacter(guy);
            }
        }

        // Update is called once per frame
        void Update()
        {

            //handle input
            //move camera
            var cameraX = camera.transform.position.x + CamVelocity * Input.GetAxis("Horizontal") * Time.deltaTime;
            var cameraY = camera.transform.position.y + CamVelocity * Input.GetAxis("Vertical") * Time.deltaTime;
            camera.transform.position = new Vector3(cameraX, cameraY, camera.transform.position.z);
            camera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * CamZoomSpeed * Time.deltaTime;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 2, CamMaxZoom);

            if (Input.GetMouseButtonDown(0))
            {

            }
        }


    }
}
