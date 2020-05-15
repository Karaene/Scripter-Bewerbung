using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class SpatialHash
    {
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int Cellsize { get; set; }

        private Dictionary<int, List<GameObject>> Buckets;

        public void Setup(int cellsize)
        {
            Cols = LevelManager.LEVEL_WIDTH / cellsize;
            Rows = LevelManager.LEVEL_HEIGHT / cellsize;
            Cellsize = cellsize;
            Buckets = new Dictionary<int, List<GameObject>>(Cols * Rows);

            for (int i = 0; i < Cols*Rows; i++)
            {
                Buckets.Add(i, new List<GameObject>());
            }
        }

        internal void ClearBuckets()
        {
            Buckets.Clear();
            for (int i = 0; i < Cols*Rows; i++)
            {
                Buckets.Add(i, new List<GameObject>());
            }
        }

        internal void RegisterObject(GameObject obj)
        {
            List<int> cellIDs = GetIdForObj(obj);
            foreach (var item in cellIDs)
            {
                if (Buckets.ContainsKey(item))
                {
                    Buckets[item].Add(obj);
                }
                
            }
        }

        private List<int> GetIdForObj(GameObject obj)
        {
            List<int> bucketsObjIsIn = new List<int>();

            Vector2D min = new Vector2D(
            obj.Transform.Position.X - (obj.Transform.Size.Width / 2),
            obj.Transform.Position.Y - (obj.Transform.Size.Height / 2));

            Vector2D max = new Vector2D(
                obj.Transform.Position.X + (obj.Transform.Size.Width / 2),
                obj.Transform.Position.Y + (obj.Transform.Size.Height / 2));    

            float width = LevelManager.LEVEL_WIDTH / Cellsize;
            //TopLeft
            AddBucket(min, width, bucketsObjIsIn);
            //TopRight
            AddBucket(new Vector2D(max.X, min.Y), width, bucketsObjIsIn);
            //BottomRight
            AddBucket(new Vector2D(max.X, max.Y), width, bucketsObjIsIn);
            //BottomLeft
            AddBucket(new Vector2D(min.X, max.Y), width, bucketsObjIsIn);

            return bucketsObjIsIn;
        }

        private void AddBucket(Vector2D vector, float width, List<int> bucketToAddTo)
        {
            int cellPosition = (int)(
                       (Math.Floor(vector.X / Cellsize)) +
                       (Math.Floor(vector.Y / Cellsize)) *
                       width
            );
            if (cellPosition < 0)
                cellPosition *= -1;
            if (!bucketToAddTo.Contains(cellPosition))
                bucketToAddTo.Add(cellPosition);

        }

        internal List<GameObject> GetNearby(GameObject obj)
        {
            List<GameObject> objects = new List<GameObject>();
            List<int> bucketIds = GetIdForObj(obj);
            foreach (var item in bucketIds)
            {
                //objects.AddRange(Buckets[item]);
                if (Buckets.ContainsKey(item))
                {
                    foreach (var go in Buckets[item])
                    {
                        if (!objects.Contains(go) && go.Active)
                            objects.Add(go);
                    }
                }
            }
            return objects;
        }
    }
}
