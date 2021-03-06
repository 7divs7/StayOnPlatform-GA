using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA 
{
    List<int> genes = new List<int>();
    int dnaLength = 0;
    int maxValues = 0; //max value that a gene can have

    public DNA(int _dnaLength, int _maxValues)
    {
        dnaLength = _dnaLength;
        maxValues = _maxValues;
        SetRandom(); //set random values to the genes
    }

    public void SetRandom()
    {
        genes.Clear();
        for(int i = 0; i < dnaLength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }

    public void Crossover(DNA d1, DNA d2)
    {
        //lower half of d1 + upper half of d2
        for(int i = 0; i < dnaLength; i++)
        {
            if(i < dnaLength/2.0f)
            {
                int c = d1.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = d2.genes[i];
                genes[i] = c;
            }
        }
    }

    public void Mutate()
    {
        genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValues);
    }

    public int GetGene(int pos)
    {
        return genes[pos];
    }
}
