using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T>
{
    //Cada Item deve controlar seu propri indece na pilha e para comparar entre dois itens e dizer qual deles tem a maior prioridade para poder classificar como heap
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items= new T[maxHeapSize];
    }

    public void Add(T item) 
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;

    }
    //Esta função é feita caso desejamos alterar a prioridade de um item
    public void UpdateItem(T item)
    {
        SortUp(item);

    }

    //Verifica e retorna o numero de itens atualmente no heap

    public int Count {
        get { return currentItemCount; }
    }

    //Verifica se a pilha contém um item especifico

    public bool Contans(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    //verifica os filhos
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            //Verificamos se este item tem pelo menos um filho (Filho da esquerda)
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    //Verificamos qual dos filhos tem uma prioridade maior
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                //Verificamos agora se o pai tem uma prioridade maior que os filhos, caso não, trocamos 
                if (item.CompareTo(items[swapIndex]) < 0) Swap(item, items[swapIndex]);
                else return;
            }
            else return;
        }
    }

    //Verifica os pais 
    void SortUp(T item) 
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem= items[parentIndex];
            //Se ele possui uma alta prioridade retonar 1, se possuir a mesma prioridade retorna 0 e se ele possuir uma baixa prioridade ele retorna -1
            /*
             * Ou seja, se o item tem uma prioridade mais lata que o do pai, que no caso particular signinifica que ele tem um custo FCost menor e então queremos troca-los
             * 
             */
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else break;
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex= itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}