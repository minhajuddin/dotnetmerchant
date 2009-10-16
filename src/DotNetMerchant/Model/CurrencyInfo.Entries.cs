#region License

// The MIT License
// 
// Copyright (c) 2009 Conatus Creative, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System.Collections.Generic;

namespace DotNetMerchant.Model
{
    partial class CurrencyInfo
    {
        private static readonly IDictionary<Currency, CurrencyInfo> _currencies
            = new Dictionary<Currency, CurrencyInfo>(2)
                  {
                      {
                          Currency.USD,
                          new CurrencyInfo
                              {
                                  DisplayName = "US Dollar",
                                  Code = Currency.USD
                              }
                          },
                      {
                          Currency.CAD,
                          new CurrencyInfo
                              {
                                  DisplayName = "Canadian Dollar",
                                  Code = Currency.CAD
                              }
                          },
                      {
                          Currency.EUR,
                          new CurrencyInfo
                              {
                                  DisplayName = "Euro",
                                  Code = Currency.EUR
                              }
                          },
                      {
                          Currency.GBP,
                          new CurrencyInfo
                              {
                                  DisplayName = "Pound Sterling",
                                  Code = Currency.GBP
                              }
                          },
                      {
                          Currency.JPY,
                          new CurrencyInfo
                              {
                                  DisplayName = "Yen",
                                  Code = Currency.JPY
                              }
                          },
                      {
                          Currency.CHF,
                          new CurrencyInfo
                              {
                                  DisplayName = "Swiss Franc",
                                  Code = Currency.CHF
                              }
                          },
                      {
                          Currency.AUD,
                          new CurrencyInfo
                              {
                                  DisplayName = "Australian Dollar",
                                  Code = Currency.AUD
                              }
                          },
                      {
                          Currency.NZD,
                          new CurrencyInfo
                              {
                                  DisplayName = "New Zealand Dollar",
                                  Code = Currency.NZD
                              }
                          },
                      {
                          Currency.INR,
                          new CurrencyInfo
                              {
                                  DisplayName = "Indian Rupee",
                                  Code = Currency.INR
                              }
                          },
                  };
    }
}