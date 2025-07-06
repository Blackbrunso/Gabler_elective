using TMPro;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    [Header("UI")]
    public GameObject bust;
    public TextMeshProUGUI score;
    [Header("PlayerMovement")]
    public PlayerMovement pM;
    [Header("GetCaught")]
    public GetCaught gC;
    [Header("Spawner")]
    public SpawnerManager sM;
    [Header("SliceObjects")]
    public ObjectSlicer Os;
    
    public float spawnrate;

    private void Update()
    {
        if(bust!= null && pM != null && gC != null&& sM!= null&& Os!= null)
        {
            if(gC.busted)
            {
                sM.enabled = false;
                pM.SetSpeed(0);
                bust.SetActive(true);
            }

            if(sM.streetGroup.maxInterval> sM.streetGroup.minInterval)
            {
                sM.streetGroup.maxInterval -= spawnrate * Time.deltaTime;
            }
            score.text = "Score: " + Os.point;
        }
    }


}
