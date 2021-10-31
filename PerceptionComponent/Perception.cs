﻿using Microsoft.ML;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using YOLOv4MLNet.DataStructures;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;
using System;
using System.IO;

namespace YOLOv4MLNet
{
    public class Perception
    {
        string modelPath;
        static public readonly string[] classesNames = new string[] { "person",
            "bicycle", "car", "motorbike", "aeroplane", "bus", "train",
            "truck", "boat", "traffic light", "fire hydrant", "stop sign",
            "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep",
            "cow", "elephant", "bear", "zebra", "giraffe", "backpack",
            "umbrella", "handbag", "tie", "suitcase", "frisbee", "skis",
            "snowboard", "sports ball", "kite", "baseball bat",
            "baseball glove", "skateboard", "surfboard", "tennis racket",
            "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl",
            "banana", "apple", "sandwich", "orange", "broccoli", "carrot",
            "hot dog", "pizza", "donut", "cake", "chair", "sofa", "pottedplant",
            "bed", "diningtable", "toilet", "tvmonitor", "laptop", "mouse",
            "remote", "keyboard", "cell phone", "microwave", "oven",
            "toaster", "sink", "refrigerator", "book", "clock", "vase",
            "scissors", "teddy bear", "hair drier", "toothbrush" };
        Microsoft.ML.PredictionEngine<YOLOv4MLNet.DataStructures.YoloV4BitmapData,
            YOLOv4MLNet.DataStructures.YoloV4Prediction> predictionEngine;
        public Perception(string modelPath)
        {
            this.modelPath = modelPath;
            MLContext mlContext = new MLContext();
            var pipeline = mlContext.Transforms.ResizeImages(inputColumnName: "bitmap", outputColumnName: "input_1:0", imageWidth: 416, imageHeight: 416, resizing: ResizingKind.IsoPad)
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input_1:0", scaleImage: 1f / 255f, interleavePixelColors: true))
                .Append(mlContext.Transforms.ApplyOnnxModel(
                    shapeDictionary: new Dictionary<string, int[]>()
                    {
                        { "input_1:0", new[] { 1, 416, 416, 3 } },
                        { "Identity:0", new[] { 1, 52, 52, 3, 85 } },
                        { "Identity_1:0", new[] { 1, 26, 26, 3, 85 } },
                        { "Identity_2:0", new[] { 1, 13, 13, 3, 85 } },
                    },
                    inputColumnNames: new[]
                    {
                        "input_1:0"
                    },
                    outputColumnNames: new[]
                    {
                        "Identity:0",
                        "Identity_1:0",
                        "Identity_2:0"
                    },
                    modelFile: modelPath, recursionLimit: 100));
            var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<YoloV4BitmapData>()));
            predictionEngine = mlContext.Model.CreatePredictionEngine<YoloV4BitmapData, YoloV4Prediction>(model);
        }
        public async Task StartPerception(Model viewmodel, string pathToImage, CancellationToken ct)
        {
                await Task.Factory.StartNew(() =>
                {
                    viewmodel.AddImage(pathToImage);
                    using (var bitmap = new Bitmap(Image.FromFile(pathToImage)))
                    {
                        YOLOv4MLNet.DataStructures.YoloV4Prediction predict;
                        lock(predictionEngine)
                        {
                            predict = predictionEngine.Predict(new YoloV4BitmapData() { Image = bitmap });
                        }
                        var results = predict.GetResults(classesNames, 0.3f, 0.7f);
                        ImgDescription imgDesc = new ImgDescription(pathToImage);
                        foreach (var res in results)
                        {
                            var x1 = res.BBox[0];
                            var y1 = res.BBox[1];
                            var x2 = res.BBox[2];
                            var y2 = res.BBox[3];
                            using (var g = Graphics.FromImage(bitmap))
                            {
                                g.DrawRectangle(Pens.Red, x1, y1, x2 - x1, y2 - y1);
                                using (var brushes = new SolidBrush(Color.FromArgb(50, Color.Red)))
                                {
                                    g.FillRectangle(brushes, x1, y1, x2 - x1, y2 - y1);
                                }
                                g.DrawString(res.Label + " " + res.Confidence.ToString("0.00"),
                                                     new Font("Arial", 12), Brushes.Blue, new PointF(x1, y1));
                            }
                            ObjDescription objDesc = new ObjDescription(x1, y1, y2 - y1, x2 - x1, res.Label);
                            imgDesc.Add(objDesc);
                        }
                        FileInfo fi = new FileInfo(pathToImage);
                        bitmap.Save(fi.DirectoryName + "\\Output" + fi.Name + "Result" + fi.Extension);
                        lock (pathToImage)
                        {
                            int ind = viewmodel.ImagesInProcess.IndexOf(pathToImage);
                            viewmodel.InsertImage(ind, fi.DirectoryName + "\\Output" + fi.Name + "Result" + fi.Extension);
                            viewmodel.RemoveImage(ind + 1);
                        }
                        if (ct.IsCancellationRequested == false)
                        {
                            viewmodel.Add(imgDesc, pathToImage);
                        }
                    }
                }, ct);
        }
        public string ModelPath
        {
            get
            {
                return modelPath;
            }
        }
    }
}