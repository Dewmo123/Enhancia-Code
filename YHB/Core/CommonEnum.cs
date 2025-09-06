namespace Assets._00.Work.YHB.Scripts.Core
{
    public static class CommonEnum
    {
        public enum DirectionInformation
        {
            Up = 1,
            Down = -1,

            Right = 2,
            Left = -2,
        }

        public static DirectionInformation Reverse(this DirectionInformation direction)
            => direction switch
            {
                DirectionInformation.Up => DirectionInformation.Down,
                DirectionInformation.Down => DirectionInformation.Up,
                DirectionInformation.Left => DirectionInformation.Right,
                DirectionInformation.Right => DirectionInformation.Left,
                _ => throw new System.NotImplementedException()
            };
    }
}
