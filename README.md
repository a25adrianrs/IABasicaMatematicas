# Cambios realizados
## Nueva escena creada a partir de la escena Villager del Repositorio
- Personaje que se puede controlar mediante teclado usando el Prefab de **Patient**
- NPCS , usando el Prefab de **Fantasmas**, los fantasmas siguen al jugador si el jugador se da la vuelta y mira directamente a los mismos,
  estos se detendran.

 ## Cree una copia del Script del PigMove al que llame *PhantomMove* en el que hacer modificaciones
  1 . Obtnemos la dirección en la que miran  tanto los "Fantasmas" como "Luigi",
  en el caso del personaje de "Luigi" aprovecho el *GameObject* que habia ya para obtener el vector de la dirección
  en la que mira el personaje controlable.
```csharp
using UnityEngine;

public class PhantomMove : MonoBehaviour
{

    // goal: obxecto destino ao que se move o porco.
    // direction: vector de dirección cara ao destino.
    // speed: velocidade de desprazamento.

    // El GameObject goal ,hace referencia al objeto "Patient" o "Luigi" en esta escena
    public GameObject goal;
```

2. Creo dos variables de Vector3 que van a almacenar la dirección a la que miran tanto los "fantasmas"  como "luigi"
  // Vectores3 donde almaceno la orientación de la vista de los fantasmas y luigi
```csharp
    Vector3 phantom_view;
    Vector3 luigi_view;
```
3. Dentro de la función existente LateUpdate usando la propiedad *Transform.forward* para obtener
el Vector3 que apunta hacia delante
```csharp
  private void LateUpdate()
    {
        // Obtengo  la dirección a la que "miran" los fantasmas
        phantom_view = transform.forward;
        Debug.Log("Dirección a la que miran los fantasmas " + phantom_view);
        // Obtengo la dirección a la que luigi "mira"
        luigi_view = goal.transform.forward;
        Debug.Log("Dirección a la que mira luigi" + luigi_view);
```
4. Obtengo el Vector3 de Luigi hasta los Fantasmas
   ```csharp
    Vector3 toPhantom = (transform.position - goal.transform.position).normalized;
   ```
5. Mediante "Dot" calcule como estaban alienados los vectores tanto de **Luigi** como de los **Fantasmas**
   ```csharp
     float dot = Vector3.Dot(luigi_view, toPhantom);
   ```
6. Por ultimo compruebo si **Luigi** mira directamente a los **Fantasmas**
   para ello compruebo el angulo en el que se miran , si se cumple la condición
   mediante **return** se sale de la función lo que provoca que los fantasmas se detengan
   mientras la orientación visual de ambos cumpla con la condición.
     ```csharp
       if (dot > 0.9f)
        {
            Debug.Log("Luigi mira fijamente a los fantasmas , estos se han congelado");
            return;
        }
```
   

