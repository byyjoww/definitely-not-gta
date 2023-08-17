using UnityEngine;
using DefinitelyNotGta.Units;
using DefinitelyNotGta.Vehicles;

namespace DefinitelyNotGta.App
{
    // Class used to perform all game initializations
    public class App : MonoBehaviour
    {
        [SerializeField] private Player player = default;
        [SerializeField] private Automobile bus = default;
        [SerializeField] private Transform[] locations = default;


        [SerializeField] Node firstNode;



        public void Start()
        {
            Init();
        }

        private void Init()
        {
            Vector3 busStopEnter = locations[0].position;
            Vector3 busStopExit = locations[1].position;
            Vector3 commuteDestination = locations[2].position;

            MoveToNextNode(firstNode);

            // player.MoveTo(busStopEnter).AddListener(delegate
            // {
            //     bus.StartDriving(player);
            //     // bus.MoveTo(busStopExit).AddListener(delegate
            //     // {
            //     //     var driver = bus.StopDriving();
            //     //     (driver as Player).MoveTo(commuteDestination).AddListener(delegate
            //     //     {
            //     //         Debug.Log($"Finished commuting");
            //     //     });
            //     // });
            // });
        }

        void MoveToNextNode(Node node)
        {
            Debug.Log($"Move To Next Node {node.name}");
            bus.MoveTo(node.transform.position).AddListener(delegate
            {
                MoveToNextNode(node.neighboors[0]);
            });
        }
    }
}