using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinitelyNotGta.Units;
using System.Linq;
using DefinitelyNotGta.Vehicles;
using DefinitelyNotGta.Environment;

namespace DefinitelyNotGta.App
{
    // Class used to perform all game initializations
    public class App : MonoBehaviour
    {
        [SerializeField] private Player player = default;
        [SerializeField] private Automobile bus = default;
        [SerializeField] private Location[] locations = default;

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            StartCoroutine(Commute());
        }

        private IEnumerator Commute()
        {
            Vector3 busStopEnter = locations[0].transform.position;
            Vector3 busStopExit = locations[1].transform.position;
            Vector3 commuteDestination = locations[2].transform.position;

            player.Move(busStopEnter);
            yield return new WaitForSeconds(10);

            bus.StartDriving(player);
            yield return new WaitForSeconds(1);

            bus.Move(busStopExit);
            yield return new WaitForSeconds(10);

            var driver = bus.StopDriving();
            yield return new WaitForSeconds(1);

            (driver as Player).Move(commuteDestination);
            yield return null;
        }
    }
}