using Eleon.Modding;
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

            //code bellow

            var infoOutLcds = CsRoot.GetDevices<ILcd>(CsRoot.Devices(E.S, "DaveSort"));
            if (infoOutLcds.Length == 0)
            {
                return;
            }

            int[] ammoSmall = { 4246, 4247, 4248, 4250, 4256, 4261, 5697, 5848, 7197, 7199, 7201, 7234 };
            int[] ammoLarge = { 4152, 4248, 4249, 4253, 4254, 4258, 4259, 4260, 4261, 4262, 4263, 4264, 4265, 4266, 4267, 4268, 5698, 5716, 5717, 5719, 5720, 5721, 5722, 5724, 5730, 5849, 7106, 7108, 7167, 7257, 7268, 5718, 5735 };
            int[] ammoHand = { 4147, 4148, 4149, 4150, 4151, 4153, 4154, 4156, 4157, 4158, 4160, 4162, 4163, 5696, 5702, 5703, 5935 };

            int[] FuelAir = { 4159, 4176, 4177, 4186, 4314, 4335, 4336, 4421, 6009 };

            int[] Deco = { 289, 330, 927, 928, 929, 1281, 1719, 2069, 2724, 2725 };
            int[] BlocksLarge = { 396, 399, 402, 405, 411, 462, 545, 1075, 1322, 1386, 1395, 1481, 2224, 2916 };
            int[] BlocksSmall = { 380, 2925, 1690, 99999999 };
            int[] ShipGuns = { 552, 683, 1650, 4196, 4197, 4198, 4199, 4200, 4201, 4202, 4203, 4204, 4205, 4206, 4207, 4208, 4209, 4210, 4211, 4211, 4212, 4213, 4214, 4215, 4216, 4217, 4218, 4219, 4221, 4222, 4223, 4224, 4225, 4226, 4227, 4228, 4229, 4230, 4231, 4232, 4233, 4234, 4236, 4237, 4238, 4239, 4272, 2170, 430, 1649, 1663, 1673, 432 };
            int[] Thrusters = { 772, 2900, 999999 };
            int[] Weapons = { 4099, 4100, 4101, 4102, 4103, 4105, 4106, 4110, 4111, 4112, 4128, 4138, 5651, 5657, 5661, 7217 };
            int[] WeaponsADV = { 4113, 4114, 4116, 4120, 4121, 4122, 4123, 4124, 4125, 4126, 4129, 4130, 4131, 4132, 4133, 4134, 4139, 4140, 4141, 4142, 4896, 5847, 7212, 7216, 7226, 7227, 7260, 7261, 4897 };
            int[] Tools = { 4098, 4104, 4115, 4117, 4119, 4127, 4136, 4137, 4796 };
            int[] Armour = { 4698, 4699, 4700, 4701, 7219, 7220 };
            int[] Boosts = { 4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4747, 4750, 4751, 4752, 5402, 5403, 5404, 7461, 7462, 7463, 7464 };
            int[] CraftingMaterials = { 4303, 4304, 4305, 4307, 4308, 4309, 4310, 4311, 4312, 4313, 4315, 4316, 4319, 4320, 4321, 4322, 4323, 4324, 4325, 4326, 4327, 4328, 4329, 4330, 4331, 4333, 4334, 4337, 4338, 4339, 4340, 4340, 4342, 4343, 4346, 4350, 4357, 4358, 4360, 4361, 4362, 4363, 4364, 4374, 4375, 4376, 4377, 4378, 4379, 4380, 4381, 4401, 2224, 4296, 4298, 4299, 4300, 4301, 4302, 4306, 4314, 4317, 4319, 4341, 5097, 5105, 5107, 5108, 5109, 5114, 5706, 5707, 5708, 5734, 5923, 5924, 5925, 5928, 5996, 5997, 6000, 6001, 6003, 6005, 6006, 7203, 7297, 7301, 7303, 7310, 7312, 7320, 7331, 7343, 7344, 7345, 7513 };
            int[] ores = { 4296, 4297, 4298, 4299, 4300, 4301, 4302, 4317, 4318, 4332, 4345, 4359, 4362, 4365 };
            int[] Food = { 4373, 4396, 4397, 4398, 4399, 4402, 4409, 4410, 4417, 4419, 4420, 4424, 4426, 4432, 4434, 4442, 4444, 4457, 4458, 4459, 4460, 4461, 4463, 4465, 4467, 4468, 4469, 4470, 4471, 4472, 4473, 4477, 4479, 4481, 4482, 4490, 5699, 7458 };
            int[] Medical = { 4403, 4404, 4423, 4425, 4430, 4433, 4437, 4441, 4464, 4474, 4475, 4476, 4478, 4483, 4484, 4485, 4486, 4487, 4488, 4489, 4493, 5715, 5904, 7316, 7446, 7448, 7450, 7452, 7453, 7454 };
            int[] Plants = { 4400, 4405, 4406, 4407, 4411, 4412, 4413, 4414, 4415, 4416, 4418, 4422, 4428, 4431, 4436, 4438, 4439, 4440, 4443, 4445, 4446, 4447, 4448, 4449, 4450, 4451, 4452, 4453, 4453, 4454, 4455, 4456, 4466, 4480, 4491, 4492, 2126 };
            int[] CVStuff = { 259, 260, 263, 266, 278, 291, 336, 343, 420, 457, 462, 468, 469, 498, 545, 564, 628, 653, 714, 717, 720, 724, 839, 934, 960, 962, 1002, 1008, 1016, 1034, 1035, 1112, 1128, 1129, 1136, 1253, 1257, 1278, 1304, 1321, 1370, 1371, 1372, 1486, 1490, 1494, 1513, 1549, 1571, 1588, 1627, 1628, 1682, 1683, 1687, 1689, 1691, 1692, 1706, 1711, 1712, 1808, 1811, 1956, 2029, 2084, 2236, 2237, 2732, 2772, 1500, 1095, 1465 };
            int[] BaseStuff = { 99999999 };
            int[] Commodities = { 4344, 4347, 4366, 4367, 5102, 5103, 5106, 5111, 5112, 5115, 5913, 5914, 5916, 5917, 7514, 7515, 997805401, 558 };
            int[] DataStuff = { 5406, 5407, 5926, 400305401, 400505401, 401105401, 60120, 5401, 5405, 640405401, 640605401 };
            int[] crap = { 4107, 4135, 4348, 4349, 4351, 4352, 4353, 4354, 4355, 4356, 4429, 2179, 5932, 5933, 1485 };

            var sItems = CsRoot.Items(E.S, "A1SortingBox");
            string temp = "";
            foreach (var item in sItems)
            {
                if (ammoHand.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp= lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                        //no idea how to make the text scroll and it lists items even if it cant move them...ie container locked...it does not dupe items though, just lists them to the lcd multiple times
                    }
                    var ammo = CsRoot.Move(item, E.S, "*ammoH*");

                    continue;
                }
                if (ammoSmall.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var ammo = CsRoot.Move(item, E.S, "*ammoS*");
                    continue;
                }
                if (ammoLarge.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var ammo = CsRoot.Move(item, E.S, "*ammoL*");
                    continue;
                }
                if (FuelAir.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var fuel = CsRoot.Move(item, E.S, "*SpareFuel*");
                    continue;
                }
                if (Deco.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var decolist = CsRoot.Move(item, E.S, "*Deco*");
                    continue;
                }
                if (BlocksLarge.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var BlocksLargelist = CsRoot.Move(item, E.S, "*BlocksLarge*");
                    continue;
                }
                if (BlocksSmall.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var BlocksSmalllist = CsRoot.Move(item, E.S, "*BlocksSmall*");
                    continue;
                }
                if (ShipGuns.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var ShipGunslist = CsRoot.Move(item, E.S, "*ShipGuns*");
                    continue;
                }
                if (Thrusters.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Thrusterslist = CsRoot.Move(item, E.S, "*Thrusters*");
                    continue;
                }
                if (Weapons.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Weaponslist = CsRoot.Move(item, E.S, "*WeaponsBox*");
                    continue;
                }
                if (WeaponsADV.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var WeaponsADVlist = CsRoot.Move(item, E.S, "*WeaponsADV*");
                    continue;
                }
                if (Tools.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Toolslist = CsRoot.Move(item, E.S, "*Tools*");
                    continue;
                }
                if (Armour.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Armourlist = CsRoot.Move(item, E.S, "*Armour*");
                    continue;
                }
                if (Boosts.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Boostslist = CsRoot.Move(item, E.S, "*Boosts*");
                    continue;
                }
                if (CraftingMaterials.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var CraftingMaterialslist = CsRoot.Move(item, E.S, "*CraftingMaterials*");
                    continue;
                }
                if (ores.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var oreslist = CsRoot.Move(item, E.S, "*oresBox*");
                    continue;
                }
                if (Food.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Foodlist = CsRoot.Move(item, E.S, "*Food*");
                    continue;
                }
                if (Medical.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Medicallist = CsRoot.Move(item, E.S, "*Medical*");
                    continue;
                }
                if (Plants.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Plantslist = CsRoot.Move(item, E.S, "*Plants*");
                    continue;
                }
                if (CVStuff.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var CVStufflist = CsRoot.Move(item, E.S, "*CVStuff*");
                    continue;
                }
                if (BaseStuff.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var BaseStufflist = CsRoot.Move(item, E.S, "*BaseStuff*");
                    continue;
                }
                if (Commodities.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var Commoditieslist = CsRoot.Move(item, E.S, "*Commodities*");
                    continue;
                }
                if (DataStuff.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var DataStufflist = CsRoot.Move(item, E.S, "*DataStuff*");
                    continue;
                }
                if (crap.Contains(item.Id))
                {
                    foreach (ILcd lcd in infoOutLcds)
                    {
                        temp = lcd.GetText();
                        lcd.SetText($"{item.Name} (ID: {item.Id}) x {item.Count}{Environment.NewLine} {temp} ");
                    }
                    var craplist = CsRoot.Move(item, E.S, "*crapBox*");
                    continue;
                }
                var miList = CsRoot.Move(item, E.S, "*DestBox*");
                miList.ForEach(mi =>
                Console.WriteLine(
                $"{mi.Id}\n" +
                $"{mi.Count}\n" +
                $"\n" +
                ""));
            }
        }

        private static void WriteTo(ILcd[] lcds, string text)
        {
            lcds.ForEach(L => L.SetText($"{text}\n{L.GetText()}"));
        }

        public void makeArray(IScriptModData g) //after 2 hours of writing out the array i realised there was an easier way...sorta
        {
            var E = g.E;
            var CsRoot = g.CsRoot;

            //code starts here
            string result = "";
            var sItems = CsRoot.Items(E.S, "A1SortingBox");
            HashSet<int> idList = new HashSet<int>();
            
            foreach(var item in sItems)
            {
                idList.Add(item.Id);
                //result += item.Id + ", ";
            }
            foreach (var item in idList)
            {
                result += item+", ";
            }
            Console.WriteLine(result);
            //ends here
        }
    }
}
