using EmpyrionScripting.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpyrionScripting.Examples.DaveCode
{
    public class DaveScripts
    {
        //sorting script
        public void Sorting(IScriptModData g)
        {
            var E = g.E;
            var CsRoot = g.CsRoot;

            //the code for the LCD named C#:ScriptName startsBellow
            int[] ammoList = { 4152, 4258, 5722, 7106 };
            int[] fuelList = { 4176, 4314, 4335, 4421 };

            var sItems = CsRoot.Items(E.S, "SourceBox");


            foreach (var item in sItems)
            {
                if (ammoList.Contains(item.Id))
                {
                    var ammo = CsRoot.Move(item, E.S, "fuelBox");
                    continue;
                }
                if (fuelList.Contains(item.Id))
                {
                    var fuel = CsRoot.Move(item, E.S, "ammoBox");
                    continue;
                }
                var miList = CsRoot.Move(item, E.S, "DestBox");
                miList.ForEach(mi =>
                Console.WriteLine(
                $"{mi.Id}\n" +
                $"{mi.Count}\n" +
                $"\n" +
                ""));
            }
            //the last line for the lcd script is one above this
        }

        public void makeArray(IScriptModData g) //after 2 hours of writing out the array i realised there was an easier way...sorta
        {
            var E = g.E;
            var CsRoot = g.CsRoot;

            //code starts here
            string result = "";
            var sItems = CsRoot.Items(E.S, "TestBox");
            foreach(var item in sItems)
            {
                result += item.Id + ", ";
            }
            Console.WriteLine(result);
            //ends here
        }
    }
}
