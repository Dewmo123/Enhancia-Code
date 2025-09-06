using UnityEngine;
using UnityEngine.Tilemaps;

namespace _00.Work.CIW._01.Scripts
{
    public enum ScanDirection
    {
        Horizontal,
        Vertical,
        Both
    }

    public class OpenGimmick : MonoBehaviour
    {
        [Header("감지 박스 설정")]
        [SerializeField] Vector2 detectBoxSize = new Vector2(2f, 2f);
        [SerializeField] LayerMask targetLayer;

        [Header("타일맵 설정")]
        [SerializeField] Tilemap gimmickMap;
        [SerializeField] int scanRadius = 1;
        [SerializeField] ScanDirection scanDirection = ScanDirection.Vertical;

        Vector3Int[] _affectedCells;
        TileBase[] _originalTiles;

        bool _initialized = false;

        private void OnEnable()
        {
            if (!_initialized)
            {
                CacheTiles();
                _initialized = true;
            }
            RestoreTiles();
        }

        private void CacheTiles()
        {
            if (gimmickMap == null)
                gimmickMap = GameObject.Find("GimmickMap").GetComponent<Tilemap>();

            Vector3Int centerCell = gimmickMap.WorldToCell(transform.position);

            switch (scanDirection)
            {
                case ScanDirection.Horizontal:
                    _affectedCells = new Vector3Int[scanRadius * 2 + 1];
                    _originalTiles = new TileBase[_affectedCells.Length];
                    for (int x = -scanRadius; x <= scanRadius; x++)
                    {
                        Vector3Int cell = centerCell + new Vector3Int(x, 0, 0);
                        int index = x + scanRadius;
                        _affectedCells[index] = cell;
                        _originalTiles[index] = gimmickMap.GetTile(cell);
                    }
                    break;

                case ScanDirection.Vertical:
                    _affectedCells = new Vector3Int[scanRadius * 2 + 1];
                    _originalTiles = new TileBase[_affectedCells.Length];
                    for (int y = -scanRadius; y <= scanRadius; y++)
                    {
                        Vector3Int cell = centerCell + new Vector3Int(0, y, 0);
                        int index = y + scanRadius;
                        _affectedCells[index] = cell;
                        _originalTiles[index] = gimmickMap.GetTile(cell);
                    }
                    break;

                case ScanDirection.Both:
                    int size = (scanRadius * 2 + 1) * (scanRadius * 2 + 1);
                    _affectedCells = new Vector3Int[size];
                    _originalTiles = new TileBase[size];
                    int i = 0;
                    for (int x = -scanRadius; x <= scanRadius; x++)
                    {
                        for (int y = -scanRadius; y <= scanRadius; y++)
                        {
                            Vector3Int cell = centerCell + new Vector3Int(x, y, 0);
                            _affectedCells[i] = cell;
                            _originalTiles[i] = gimmickMap.GetTile(cell);
                            i++;
                        }
                    }
                    break;
            }
        }

        private void Update()
        {
            // OverlapBox 감지
            Collider2D hit = Physics2D.OverlapBox(transform.position, detectBoxSize, 0f, targetLayer);

            if (hit != null)
            {
                HideTiles();
            }
            else
            {
                RestoreTiles();
            }
        }

        private void HideTiles()
        {
            foreach (var cell in _affectedCells)
            {
                gimmickMap.SetTile(cell, null);
            }
        }

        private void RestoreTiles()
        {
            for (int i = 0; i < _affectedCells.Length; i++)
            {
                gimmickMap.SetTile(_affectedCells[i], _originalTiles[i]);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, detectBoxSize);
        }

        private void OnDisable()
        {
            RestoreTiles();
        }
    }
}
