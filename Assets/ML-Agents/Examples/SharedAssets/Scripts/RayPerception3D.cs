using System.Collections.Generic;
using UnityEngine;

namespace MLAgents
{

    /// <summary>
    /// Raycast "senses"
    /// </summary>
    public class RayPerception3D : RayPerception
    {
        Vector3 endPosition;
        RaycastHit hit;


        /// <param name="rayDistance">Radius of rays</param>
        /// <param name="rayAngles">Angles of rays</param>
        /// <param name="detectableObjects">Detectable objects</param>
        /// <param name="startOffset">Starting raycast offset</param>
        /// <param name="endOffset">Ending raycast offset</param>
        public override List<float> Perceive(float rayDistance,
            float[] rayAngles, string[] detectableObjects,
            float startOffset, float endOffset)
        {
            perceptionBuffer.Clear();

            foreach (float angle in rayAngles)
            {
                endPosition = transform.TransformDirection(
                    PolarToCartesian(rayDistance, angle));
                endPosition.y = endOffset;
                if (Application.isEditor)
                {
                    Debug.DrawRay(transform.position + new Vector3(0f, startOffset, 0f),
                        endPosition, Color.black, 0.01f, true);
                }

                float[] subList = new float[detectableObjects.Length + 2];
                if (Physics.SphereCast(transform.position +
                                       new Vector3(0f, startOffset, 0f), 0.5f,
                    endPosition, out hit, rayDistance))
                {
                    for (int i = 0; i < detectableObjects.Length; i++)
                    {
                        if (hit.collider.gameObject.CompareTag(detectableObjects[i]))
                        {
                            subList[i] = 1;
                            subList[detectableObjects.Length + 1] = hit.distance / rayDistance;
                            break;
                        }
                    }
                }
                else
                {
                    subList[detectableObjects.Length] = 1f;
                }

                perceptionBuffer.AddRange(subList);
            }

            return perceptionBuffer;
        }


        public static Vector3 PolarToCartesian(float radius, float angle)
        {
            float x = radius * Mathf.Cos(DegreeToRadian(angle));
            float z = radius * Mathf.Sin(DegreeToRadian(angle));
            return new Vector3(x, 0f, z);
        }

    }
}
