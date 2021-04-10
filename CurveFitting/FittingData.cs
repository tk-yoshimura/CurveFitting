namespace CurveFitting {

    /// <summary>フィッティング関数真値</summary>
    public struct FittingData {

        /// <summary>コンストラクタ</summary>
        public FittingData(double x, double y) {
            this.X = x;
            this.Y = y;
        }

        /// <summary>独立変数</summary>
        public double X { get; set; }

        /// <summary>従属変数</summary>
        public double Y { get; set; }

        /// <summary>文字列化</summary>
        public override string ToString() {
            return $"{X},{Y}";
        }
    }
}