using UnityEngine;

namespace ThingSpace
{
    // The ClassName will be replaced by the student's input.
    public class Yang : Thing
    {
        public override void Init()
        {
            // Initialization code goes here.
            // assign values to all the existing variables. including red gree and blue.
            // be sure to distinguish double and float types

            // set the mesh index to 21 (provided by user)
            meshIndex = 21;

            // set the vertex count to 50 (provided by user)
            

            // set the speed to 10 (provided by user)
            speed = 10;

            // set the cohesion to 5 (provided by user)
            cohesion = 5;

            // set the separation to 5 (provided by user)
            seperation = 5;

            // set the spikiness to 0.5 (provided by user)
            spikyness = 0.5f;

            // set the width, height and depth to 3 (provided by user)
            width = height = depth = 3;

            // set the color to #808080 (provided by user)
            string hex = "#808080";


            // set the red, green and blue properties (provided by user)
            red = 0.502;
            green = 0.502;
            blue = 0.502;

            //set color
            Color color = new Color((float)red, (float)green, (float)blue);
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);

            foreach (var mat in GetComponent<MeshRenderer>().materials)
            {
                mat.SetColor("_ColorA1", color);
       
            }

            // Implement other behaviors as defined by the user.
        }

        public override void OnTouch(Thing other)
        {
            // Implement the OnTouch behavior as defined by the user.
        }

        public override void IntervalAction(Thing closestThing)
        {
            // Implement the IntervalAction behavior as defined by the user.
        }
    }
}
