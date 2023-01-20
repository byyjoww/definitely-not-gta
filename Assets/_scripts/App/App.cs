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
        [SerializeField] private Automobile automobile = default;
        [SerializeField] private Location[] locations = default;

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            StartCoroutine(SampleTasks());
        }

        private IEnumerator SampleTasks()
        {
            player.Move(locations[0].transform.position);
            yield return new WaitForSeconds(10);
            automobile.StartDriving(player);
            yield return new WaitForSeconds(1);
            automobile.Move(locations[1].transform.position);
            yield return new WaitForSeconds(10);
            automobile.StopDriving();
            yield return new WaitForSeconds(1);
            yield return null;
        }
    }
}