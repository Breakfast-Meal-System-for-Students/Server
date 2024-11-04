using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.AI
{
    public class RecommendationEngine
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public RecommendationEngine()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(List<ProductEntry> purchaseData)
        {
            var dataView = _mlContext.Data.LoadFromEnumerable(purchaseData);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "UserIdEncoded", inputColumnName: "UserId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "ProductIdEncoded", inputColumnName: "ProductId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                    labelColumnName: "Label",
                    matrixColumnIndexColumnName: "UserIdEncoded",
                    matrixRowIndexColumnName: "ProductIdEncoded"));

            _model = pipeline.Fit(dataView);

            _mlContext.Model.Save(_model, dataView.Schema, "model.zip");

        }

        public List<ProductPrediction> PredictForUser(int userId, IEnumerable<int> productIds)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductEntry, ProductPrediction>(_model);

            var predictions = productIds.Select(productId => predictionEngine.Predict(
                new ProductEntry
                {
                    UserId = (float)userId,
                    ProductId = (float)productId
                })).ToList();

            return predictions;
        }

    }

    public class ProductEntry
{
    public float UserId { get; set; }
    public float ProductId { get; set; }
    public float Label { get; set; }  // Rating or Purchase Count
}

public class ProductPrediction
{
    public float ProductId;
    public float Score;
}
}
