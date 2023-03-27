using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTrack : MonoBehaviour
{
    private GameObject[] viewTracks;
    private int trackCount;
    private float viewWidth;
    [SerializeField]
    private List<float> speedMultiplier;
    [SerializeField]
    private Transform objectToFollow;

    /*
    The anchor of the parent track object should locate on the left edge, such that the left edge is alligned with x=0
    */
    void Awake()
    {
        trackCount = transform.childCount;
        if(trackCount != speedMultiplier.Count || trackCount != speedMultiplier.Count)
        {
            Debug.LogError("ParallaxTracks: follow speed not properly set");
        }

        viewTracks = new GameObject[trackCount];
        for(int i = 0; i < trackCount; i++)
        {
            viewTracks[i] = transform.GetChild(i).gameObject;
        }
        viewWidth = GetComponentInChildren<SpriteRenderer>().size.x;
    }
    void Start()
    {
        float viewportHeight = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        if(objectToFollow == null)
        {
            objectToFollow = Camera.main.transform;
        }
    }
    private void FixedUpdate()
    {
        VerticalTransform();
        GalileanTransform();
    }
    private void VerticalTransform()
    {
        float playerPosWRTGround = objectToFollow.position.y;
        for (int i = 0; i < trackCount; i++)
        {
            //calculate the player's position wrt track origin
            float playerPosWRTTrack = playerPosWRTGround * speedMultiplier[i];
            //calculate the origin of the track
            float TrackWRTGround = playerPosWRTGround - playerPosWRTTrack;
            //move the origin of the track
            viewTracks[i].transform.localPosition = new Vector3(viewTracks[i].transform.localPosition.x, Mathf.Max(0, TrackWRTGround), viewTracks[i].transform.localPosition.z); 
        }
    }
    public void GalileanTransform()
    {
        float playerPosWRTGround = objectToFollow.transform.position.x;
        float viewportWidth = Camera.main.ViewportToWorldPoint(new Vector2(1,0)).x - Camera.main.ViewportToWorldPoint(new Vector2(0,0)).x;
        for(int i = 0; i < trackCount; i++)
        {
            //calculate the player's position with respect to track origin
            float playerPosWRTTrack = playerPosWRTGround * speedMultiplier[i];
            //calculate the origin of the track
            float TrackWRTGround = playerPosWRTGround - playerPosWRTTrack;
            //move the origin of the track
            viewTracks[i].transform.localPosition = new Vector3(TrackWRTGround, viewTracks[i].transform.localPosition.y, viewTracks[i].transform.localPosition.z);
            //At how many images widths is the viewport edge located
            Vector2 left = new Vector2(TrackWRTGround, 0);
            //Debug.DrawRay(left, Vector2.up * 100f, Color.blue);
            int stepToViewportLeft = (int) Mathf.Floor((playerPosWRTTrack - viewportWidth/2f + viewWidth/2)/viewWidth);
            int stepToViewportRight = (int) Mathf.Floor((playerPosWRTTrack + viewportWidth/2f + viewWidth/2)/viewWidth);
            //check if the you need more images to fill the viewport gap
            if(stepToViewportRight - stepToViewportLeft >= viewTracks[i].transform.childCount)
            {
                //if gap is 2, you need 3 images to fill
                for (int j = viewTracks[i].transform.childCount; j < stepToViewportRight - stepToViewportLeft + 1; j++)
                {
                    Instantiate(viewTracks[i].transform.GetChild(0).gameObject, viewTracks[i].transform);
                }
            }
            //put every view on track
            for (int j = 0; j < viewTracks[i].transform.childCount; j++)
            {
                if(j > stepToViewportRight - stepToViewportLeft)
                {
                    viewTracks[i].transform.GetChild(j).gameObject.SetActive(false);
                    continue;
                }
                float viewPositionWRTTrack = (j + stepToViewportLeft) * viewWidth;
                viewTracks[i].transform.GetChild(j).localPosition = new Vector3(viewPositionWRTTrack, viewTracks[i].transform.GetChild(j).localPosition.y, viewTracks[i].transform.GetChild(j).localPosition.z);
                viewTracks[i].transform.GetChild(j).gameObject.SetActive(true);             
            }
        }
    }
}


