using UnityEngine;

namespace utiles
{
    public class Effect: MonoBehaviour
    {
        public static void ClickEffect(Vector2 position, Color color)
        {
            var circle = new Circle(0.2f, position);
            float alpha = 0;
            for (var i = 0; i < 8; ++i)
            {
                var circleGo = Instantiate(Resources.Load("Prefabs/circle") as GameObject);
                circleGo.GetComponent<SpriteRenderer>().color = color;
                circleGo.transform.SetPositionAndRotation(circle.GetPointFromAngle(alpha),
                    Quaternion.Euler(0, 0, Mathf.Rad2Deg * alpha));
                circleGo.transform.localScale = new Vector2(0.12f, 0.12f);

                var direction = circle.GetPointFromAngle(alpha) - circle.Center;
                circleGo.GetComponent<Rigidbody2D>().velocity = direction * CircleScript.Velocity;
                circleGo.GetComponent<Rigidbody2D>().AddForce(-direction * 1600f);

                alpha += 2 * Mathf.PI / 8;
            }
        }

    }
}