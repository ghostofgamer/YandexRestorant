using UnityEngine;

namespace CityTrafficContent
{
    public class NPCTraffic : CityTraffic<PeopleNpc>
    {
        public override void Init(PeopleNpc gameNPC, GameObject path, CityTraffic<PeopleNpc> cityTraffic)
        {
            gameNPC.Init(path,cityTraffic);
            gameNPC.InitUniqueData();
        }
    }
}