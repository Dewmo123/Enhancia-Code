using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets._00.Work.YHB.Scripts.Others
{
    public class TextObject : MonoBehaviour
    {
        [Header("Layer")]
#if UNITY_EDITOR
        [SerializeField] protected string sortLayerName;
#endif
        [SerializeField] protected int layerNumber = 0;

        protected MeshRenderer _meshRenderer;
        protected TextMeshPro _text;

        protected virtual void Awake()
        {
            _meshRenderer = transform.GetComponent<MeshRenderer>();
            _text = transform.GetComponent<TextMeshPro>();

            _meshRenderer.sortingLayerID = layerNumber;
            _meshRenderer.sortingOrder = 10;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            try
            {
                _meshRenderer = transform.GetComponent<MeshRenderer>();
                _meshRenderer.sortingLayerName = sortLayerName;
            }
            catch
            {
                _meshRenderer.sortingLayerID = layerNumber;
                return;
            }

            layerNumber = _meshRenderer.sortingLayerID;
        }
#endif
    }
}
