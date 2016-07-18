using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OsmSharp;
using OsmSharp.Math.Geo;
using OsmSharp.Osm;
using OsmSharp.Osm.Xml.Streams;
using OsmSharp.UI.Map;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Map.Styles;
using OsmSharp.WinForms.UI;

namespace osmtest
{
    public partial class Form1 : Form
    {
        MapControl mapControl;
        LayerPrimitives layer;

        public Form1()
        {
            InitializeComponent();
        }

        private void XAPIbutton_Click(object sender, EventArgs e)
        {
            DateTime n = DateTime.Now;

            var query = "way[building=*][bbox=27.48017,53.93668,27.48516,53.93945]";
            var request = WebRequest.Create("http://overpass.osm.rambler.ru/cgi/xapi_meta?" + query);
            request.Method = "GET";
            var requestStream = request.GetResponse().GetResponseStream();
            var source = new XmlOsmStreamSource(requestStream);

            var osmEntities = new List<OsmGeo>(source);
            var outputCsv = new List<string[]>();
            for (int idx = 0; idx < osmEntities.Count; idx++)
            {
                var node = osmEntities[idx] as Node;
                var refId = string.Empty;
                var name = string.Empty;
                if (node != null)
                {
                    var latitude = node.Coordinate.Latitude;
                    var longitude = node.Coordinate.Longitude;

                    outputCsv.Add(new string[] { latitude.ToInvariantString(), longitude.ToInvariantString() });
                }
            }

            foreach (var item in outputCsv)
            {
                textBox2.Text += item[0];
                textBox2.Text += ' ';
                textBox2.Text += item[1];
                textBox2.Text += Environment.NewLine;
            }

            label2.Text = (DateTime.Now - n).ToString();

            var map = new Map();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var map = new Map();
            layer = new LayerPrimitives(new OsmSharp.Math.Geo.Projections.WebMercator());
            layer.AddPoint(new GeoCoordinate(53.9384963, 27.483288), 1, 1);
            layer.AddPoint(new GeoCoordinate(53.9386293, 27.4834595), 1, 2);
            layer.AddPoint(new GeoCoordinate(53.9385067, 27.4837339), 1, 3);
            layer.AddPoint(new GeoCoordinate(53.9383737, 27.4835625), 1, 4);
            
            map.AddLayer(layer);
            layer.AddLine(new GeoCoordinate(53.9384963, 27.483288), new GeoCoordinate(53.9386293, 27.4834595), 1, 2);

            mapControl = new MapControl();
            mapControl.Location = new Point(10, 10);
            mapControl.Size = new Size(500, 500);
            mapControl.BackColor = Color.Pink;

            mapControl.Map = map;
            mapControl.MapCenter = new GeoCoordinate(53.9385, 27.4835);
            mapControl.MapZoom = 18;
            mapControl.Refresh();
        
            


            Controls.Add(mapControl);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rand = new Random();
            var dx = (rand.NextDouble() - 0.5) / 1000;
            var dy = (rand.NextDouble() - 0.5) / 1000;
            layer.AddPoint(new GeoCoordinate(mapControl.MapCenter.Latitude + dx, mapControl.MapCenter.Longitude + dy), 5, 1);
        }
    }
}
