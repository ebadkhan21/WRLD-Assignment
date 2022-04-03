using Wrld;
using Wrld.Space;
using UnityEngine;
using Wrld.Transport;
using System.Threading.Tasks;
using Assets.Wrld.Scripts.Maths;
using System.Collections.Generic;

[System.Serializable]
public class EnemyPos
{
    public double Latitude;
    public double Longitude;
    public LatLong latLong => LatLong.FromDegrees(Latitude, Longitude);
    public GeographicTransform EnemyMade = null;

    public EnemyPos(double latitude = 37.795159, double longitude = -122.404336)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class EnemyMaker : MonoBehaviour
{
    [SerializeField] Shooter shooter = null;
    [SerializeField] GeographicTransform Prefab = null;
    [SerializeField] List<EnemyPos> EnemiesToMake = new List<EnemyPos>();

    private async void Start()
    {
        await Task.Delay(6000);
        for (int i = 0; i < EnemiesToMake.Count; i++)
        {
            MakeEnemy(i);
        }
        StartCoroutine(shooter.CustomUpdate());
    }

    private void MakeEnemy(int index)
    {
        DoubleRay ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(EnemiesToMake[index].latLong);
        LatLongAltitude buildingIntersectionPoint;
        bool didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        print(didIntersectBuilding);
        if (didIntersectBuilding)
        {
            EnemiesToMake[index].EnemyMade = Instantiate(Prefab);
            EnemiesToMake[index].EnemyMade.gameObject.name = $"Enemy_{index}";
            EnemiesToMake[index].EnemyMade.SetPosition(buildingIntersectionPoint.GetLatLong());
            EnemiesToMake[index].EnemyMade.transform.localPosition = Vector3.up * (float)buildingIntersectionPoint.GetAltitude();
            if (EnemiesToMake[index].EnemyMade.TryGetComponent<Enemy>(out Enemy enemy))
                shooter.AddEnemy(enemy);
        }
    }

}