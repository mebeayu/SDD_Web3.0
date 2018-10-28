﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace Aliyun.Acs.Core.Auth
{
    public static class AcsUrlEncoder
    {
        private const string EncodingUtf8 = "UTF-8";

        public static string Encode(string value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8);
        }

        public static string PercentEncode(String value)
        {
            var stringBuilder = new StringBuilder();
            const string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            var bytes = Encoding.GetEncoding(EncodingUtf8).GetBytes(value);
            foreach (var b in bytes)
            {
                var c = (char) b;
                if (text.IndexOf(c) >= 0)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("%").Append(
                        string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                }
            }
            return stringBuilder.ToString();
        }
    }
}