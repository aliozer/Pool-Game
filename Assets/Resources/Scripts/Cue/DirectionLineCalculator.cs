using AO;
using AO.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class DirectionLineCalculator : BaseMonoBehaviour
    {
        public GameObject LineRendererPrefab;

        private RaycastHit hit;
        private Ray ray;
        private Color lineRendererColor;
        private GameObject lineRendererContainerGO;
        private Vector3 inDirection = Vector3.zero;
        private float maxReflectDistance = 10.0f;
        private int reflectionCount = 1;

        [SerializeField]
        private float maxDistance = 100.0f;

        [SerializeField]
        private float radius = 0.5f;

        [SerializeField]
        private GameObject projection;

        public GameObject Projection {
            get { return projection; }
            set {
                projection = value;

                if (projection != null)
                {
                    projection = Instantiate(projection);
                    projection.SetActive(false);
                }
            }
        }

        public float Radius {
            get { return radius; }
            set { radius = value; }
        }


        public IDirection DirectionObject { get; set; }
        public Vector3 StartingPosition { get; set; }


        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            ray = new Ray(StartingPosition, DirectionObject.GetDirection());

            Debug.DrawRay(StartingPosition, DirectionObject.GetDirection().normalized * maxDistance, Color.magenta);

            var lineRenderer = GetLineRenderer("0");
            lineRenderer.SetPosition(0, StartingPosition);

            if (Physics.SphereCast(ray, Radius, out hit, maxDistance))
            {
                Vector3 hitPoint = Vector3.MoveTowards(hit.point, hit.point + hit.normal, radius);

                if (Projection != null)
                {
                    Projection.transform.position = hitPoint;
                }

                if (hit.collider.tag.Equals(Tags.Wall.GetStringValue()))
                {
                    RemoveAllLineRendererExceptFirst();

                    lineRenderer = GetLineRenderer("0");
                    lineRenderer.SetPosition(1, hitPoint);
                }
                else
                {
                    for (int i = 0; i <= reflectionCount; i++)
                    {

                        if (Physics.SphereCast(ray, radius, out hit, maxDistance))
                        {
                            hitPoint = Vector3.MoveTowards(hit.point, hit.point + hit.normal, radius);
                            inDirection = Vector3.Reflect(ray.direction, hit.normal);

                            if (i == reflectionCount)
                            {
                                lineRenderer = GetLineRenderer(i.ToString());


                                if (lineRenderer != null)
                                {
                                    if (Vector3.Distance(lineRenderer.GetPosition(0), hit.point) < maxReflectDistance)
                                    {
                                        lineRenderer.SetPosition(1, hitPoint);
                                    }

                                    break;
                                }
                            }


                            ray = new Ray(hitPoint, inDirection);

                            Debug.DrawRay(hit.point, hit.normal * 3, Color.blue);
                            Debug.DrawRay(hitPoint, inDirection.normalized * maxReflectDistance, Color.green);


                            if (GetLineRenderer((i + 1).ToString()) == null)
                            {
                                CreateLineRenderer((i + 1).ToString(), lineRendererContainerGO);
                            }

                            lineRenderer = GetLineRenderer((i + 1).ToString());

                            lineRenderer.SetPosition(0, hitPoint);
                            lineRenderer.SetPosition(1, hitPoint + inDirection.normalized * maxReflectDistance);

                            lineRenderer = GetLineRenderer(i.ToString());
                            lineRenderer.SetPosition(1, hitPoint);


                        }
                        else
                        {
                            lineRenderer = GetLineRenderer((i + 1).ToString());

                            if (lineRenderer != null)
                            {
                                Object.Destroy(lineRenderer.gameObject);
                            }

                        }

                    }
                }

            }


        }

    
        private void CreateLineRendererContainerGO()
        {
            lineRendererContainerGO = new GameObject("LineRendererContainerGO");
            CreateLineRenderer("0", lineRendererContainerGO);
        }

        public override void SetEnabled(bool enabled)
        {

            if (!enabled)
            {
                Object.DestroyImmediate(lineRendererContainerGO);
            }
            else
            {
                CreateLineRendererContainerGO();
            }

            if (Projection != null)
            {
                Projection.SetActive(enabled);
            }

            base.SetEnabled(enabled);
        }


        private void RemoveAllLineRendererExceptFirst()
        {
            if (lineRendererContainerGO != null)
            {
                int count = lineRendererContainerGO.transform.childCount;

                while (count > 1)
                {
                    var lineRenderer = GetLineRenderer((count - 1).ToString());

                    if (lineRenderer != null)
                    {
                        Object.Destroy(lineRenderer.gameObject);
                        count--;
                    }
                }

            }
        }

        private LineRenderer GetLineRenderer(string name)
        {
            if (lineRendererContainerGO.transform.Find(name) == null)
                return null;

            return lineRendererContainerGO.transform.Find(name).GetComponent<LineRenderer>();
        }

        private LineRenderer CreateLineRenderer(string name, GameObject parent)
        {
            var lineRenderer = Instantiate(LineRendererPrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            lineRenderer.name = name;
            lineRenderer.transform.parent = parent.transform;
            return lineRenderer;
        }

        public void Stop()
        {
            if (lineRendererContainerGO != null)
            {
                Object.DestroyImmediate(lineRendererContainerGO);
                lineRendererContainerGO = null;
            }

            if (Projection != null)
            {
                Object.DestroyImmediate(Projection);
                Projection = null;
            }
        }
    }
}
