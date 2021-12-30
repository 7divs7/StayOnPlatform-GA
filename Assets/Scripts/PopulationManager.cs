using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int PopulationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trialTime = 10;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 250, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 250, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 250, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

    // Start is called before the first frame update, USED for initialization
    void Start()
    {
        for(int i = 0; i < PopulationSize; i++)
        {
            Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
            GameObject go = Instantiate(botPrefab, startingPos, this.transform.rotation);
            go.GetComponent<Brain>().Init();
            population.Add(go);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, this.transform.position.z + Random.Range(-2, 2));
        GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
        Brain b = offspring.GetComponent<Brain>();
        b.Init();
        if(Random.Range(0, 100) == 1) // mutate 1 in 100
        {
            b.dna.Mutate();
        }
        else
        {
            b.dna.Crossover(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }

    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeWalking * o.GetComponent<Brain>().timeAlive).ToList();

        population.Clear();
        for(int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count -1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i+1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
        }

        //destroy all parent and previous population
        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
