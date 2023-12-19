using UnityEngine;

namespace Menu
{
    public class RopeController : MonoBehaviour
    {
        public Transform point1; // Точка 1 (начальная точка верёвки)
        public Transform point2; // Точка 2 (конечная точка верёвки)
        private GameObject ropeModel; // Объект с верёвкой (ваш узкий GameObject)
        private Vector3 initialScale; // Исходный размер объекта

        void Start()
        {
            // Получаем ссылку на объект с верёвкой
            ropeModel = gameObject;
            initialScale = ropeModel.transform.localScale;

            UpdateRope();
        }

        void Update()
        {
            UpdateRope();
        }

        void UpdateRope()
        {
            if (point1 != null && point2 != null && ropeModel != null)
            {
                // Расчет направления между точками
                Vector3 direction = point2.position - point1.position;

                // Позиция объекта - середина между точками
                ropeModel.transform.position = (point1.position + point2.position) / 2f;

                // Растягивание объекта от точки 1 к точке 2
                ropeModel.transform.localScale = new Vector3(direction.magnitude , initialScale.y, initialScale.z);

                // Угол между направлением и осью X (например, для 2D)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
                // Устанавливаем поворот вокруг оси Z
                ropeModel.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}