using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(-1,1f)]
    //[SerializeField] private float scrollSpeed = 0.5f;
    private float _offset;
    private Material _backgroundMaterial;

    [SerializeField] private RawImage _bgImg;
    [SerializeField] private float _x, _y;

    void Start()
    {
        //_backgroundMaterial= GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        /*_offset += (Time.deltaTime * scrollSpeed) / 10f;
        _backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(_offset, 0));*/

        _bgImg.uvRect = new Rect(_bgImg.uvRect.position + new Vector2(_x, _y) * Time.deltaTime,_bgImg.uvRect.size);
    }
}
