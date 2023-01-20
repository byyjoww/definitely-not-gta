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
            Vector3 busStopEnter = locations[0].transform.position;
            Vector3 busStopExit = locations[1].transform.position;
            Vector3 commuteDestination = locations[2].transform.position;

            player.Move(busStopEnter).AddListener(delegate 
            {
                bus.StartDriving(player);
                bus.Move(busStopExit).AddListener(delegate 
                {
                    var driver = bus.StopDriving();
                    (driver as Player).Move(commuteDestination).AddListener(delegate 
                    {
                        Debug.Log($"Finished commuting");
                    });
                });
            });
        }
    }
}