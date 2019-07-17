using Microsoft.AspNetCore.Builder;
using Prometheus;
using System;

namespace Digipolis.Prometheus
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// This adds Prometheus Metric Server to the Microsoft.AspNetCore.Builder.IApplicationBuilder request execution pipeline;
        /// Call this method as the first statement in StartUp.Configure for accurate metrics;
        /// The order in which the Use-statements are added, determines the sequence of middleware execution
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app, string route = "/metrics")
        {
            var counter = Metrics.CreateCounter("http_request_count", "Number of HTTP requests", new CounterConfiguration
            {
                LabelNames = new[] { "method", "route", "code" }
            });

            var latency = Metrics.CreateHistogram("http_request_duration_ms", "Duration of HTTP requests in ms", new HistogramConfiguration
            {
                LabelNames = new[] { "method", "route", "code" },
                Buckets = new double[] { 0.10, 5, 15, 50, 100, 200, 300, 400, 500 }
            });
            
            // apply custom metrics
            app.Use(async (context, next) =>
            {                
                var startTime = DateTime.Now;

                await next();

                // counter metric: call status
                counter.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Inc();

                // histogram metric: call duration
                var duration = DateTime.Now - startTime;
                latency.WithLabels(context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString()).Observe(duration.TotalMilliseconds);             
            });

            Console.WriteLine("Start Prometheus metrics server");

            return app.UseMetricServer(route);
        }

    }
}
