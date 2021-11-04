namespace Assets.Scripts
{
    public class Tetrominoe
    {
        public int X;
        public int Y;
        public int[][] Shape;
        public bool Active;

        private static int[][] shapeI =
        {
            new int[]{0, 0, 0, 0 },
            new int[]{1, 1, 1, 1 },
            new int[]{0, 0, 0, 0 },
            new int[]{0, 0, 0, 0 }
        };

        private static int[][] shapeL =
        {
            new int[]{2, 0, 0 },
            new int[]{2, 2, 2 },
            new int[]{0, 0, 0 }
        };

        private static int[][] shapeJ =
        {
            new int[]{0, 0, 0 },
            new int[]{3, 3, 3 },
            new int[]{3, 0, 0 }
        };

        private static int[][] shapeS =
        {
            new int[]{0, 4, 4 },
            new int[]{4, 4, 0 },
            new int[]{0, 0, 0 }
        };

        private static int[][] shapeZ =
        {
            new int[]{5, 5, 0 },
            new int[]{0, 5, 5 },
            new int[]{0, 0, 0 }
        };

        private static int[][] shapeO =
        {
            new int[]{6, 6 },
            new int[]{6, 6 }
        };

        private static int[][] shapeT =
        {
            new int[]{0, 7, 0 },
            new int[]{7, 7, 7 },
            new int[]{0, 0, 0 }
        };

        public static int[][][] Shapes =
        {
            shapeI,
            shapeL,
            shapeJ,
            shapeS,
            shapeZ,
            shapeO,
            shapeT
        };

        public static Tetrominoe GetTetrominoe(int index)
        {
            return new Tetrominoe(Shapes[index]);
        }

        private Tetrominoe(int[][] shape)
        {
            Active = true;
            Shape = shape;
            X = 3;
            Y = 0;
        }
    }
}
