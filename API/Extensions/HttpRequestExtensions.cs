// <copyright file="HttpRequestExtensions.cs" company="CS:GO Tunes">
// Copyright (c) CS:GO Tunes. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using RealGoodApps.ValueImmutableCollections;

namespace CSGOTunes.API.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Read and validate the incoming request body.
        /// </summary>
        /// <param name="req">The incoming request.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <typeparam name="TRequest">The incoming request type.</typeparam>
        /// <returns>The deserialized request and the model state dictionary with validation results.</returns>
        public static async Task<(TRequest? Request, ModelStateDictionary ModelState)> ReadAndValidateAsync<TRequest>(
            this HttpRequest req,
            CancellationToken cancellationToken)
            where TRequest : class
        {
            var requestJson = await req.ReadAsStringAsync();

            TRequest? requestModel;
            var modelState = new ModelStateDictionary();

            try
            {
                requestModel = JsonConvert.DeserializeObject<TRequest>(requestJson);
            }
            catch (Exception)
            {
                modelState.AddModelError(string.Empty, "The request is no formatted properly.");
                return (null, modelState);
            }

            if (requestModel == null)
            {
                modelState.AddModelError(string.Empty, "Your request must not be empty.");
                return (null, modelState);
            }

            var context = new ValidationContext(requestModel, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(requestModel, context, validationResults, true);

            var validationResultsValueImmutableList = validationResults.ToValueImmutableList();

            foreach (var validationResult in validationResultsValueImmutableList)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    modelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }

            return (requestModel, modelState);
        }
    }
}
