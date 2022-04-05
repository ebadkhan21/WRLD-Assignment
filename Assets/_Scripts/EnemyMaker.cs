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


[System.Serializable]
public class GroundEnemyPos
{
    public double Latitude;
    public double Longitude;
    public double Elevation;

    public LatLongAltitude latLong => LatLongAltitude.FromDegrees(Latitude, Longitude, Elevation);
    public GameObject EnemyMade = null;

    public GroundEnemyPos(double latitude = 37.784468, double longitude = -122.401268, double elevation = 10.0)
    {
        Latitude = latitude;
        Longitude = longitude;
        Elevation = elevation;
    }
}



public class EnemyMaker : MonoBehaviour
{
    [SerializeField] Shooter shooter = null;
    [SerializeField] GeographicTransform Prefab = null;
    [SerializeField] GameObject enemyPrefabGround;
    [SerializeField] List<EnemyPos> EnemiesToMake = new List<EnemyPos>();
    [SerializeField] List<GroundEnemyPos> GroundEnemiesToMake = new List<GroundEnemyPos>();

    private async void Start()
    {
        await Task.Delay(6000);
        for (int i = 0; i < EnemiesToMake.Count; i++)
        {
            MakeEnemy(i);
        }
        for (int i = 0; i < GroundEnemiesToMake.Count; i++)
        {
            MakeEnemyOnGround(i);
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

    private void MakeEnemyOnGround(int index)
    {
        var position = Api.Instance.SpacesApi.GeographicToWorldPoint(GroundEnemiesToMake[index].latLong);
        GroundEnemiesToMake[index].EnemyMade = Instantiate(enemyPrefabGround);
        GroundEnemiesToMake[index].EnemyMade.gameObject.name = $"Enemy_{index}";
        GroundEnemiesToMake[index].EnemyMade.transform.localPosition = position;
        if (GroundEnemiesToMake[index].EnemyMade.TryGetComponent<Enemy>(out Enemy enemy))
            shooter.AddEnemy(enemy);
    }

}