using UnityEngine;

namespace CityTrafficContent
{
    public class CarTraffic : CityTraffic<CarNPC>
    {
        public override void Init(CarNPC gameNPC, GameObject path, CityTraffic<CarNPC> cityTraffic)
        {
            gameNPC.Init(path,cityTraffic);
            gameNPC.InitUniqueData();
        }
    }
}
