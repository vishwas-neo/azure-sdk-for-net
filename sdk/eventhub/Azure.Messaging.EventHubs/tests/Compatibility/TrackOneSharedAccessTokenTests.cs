﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Messaging.EventHubs.Authorization;
using Azure.Messaging.EventHubs.Compatibility;
using NUnit.Framework;
using TrackOne;

namespace Azure.Messaging.EventHubs.Tests
{
    /// <summary>
    ///   The suite of tests for the <see cref="TrackOneSharedAccessSignatureToken" />
    ///   class.
    /// </summary>
    ///
    [TestFixture]
    public class TrackOneSharedAccessSignatureTokenTests
    {
        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorValidatesTheSignature()
        {
            Assert.That(() => new TrackOneSharedAccessSignatureToken(null), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorValidatesTheSignatureValue()
        {
            var signature = new SettablePropertiesMock("keyName", "key", DateTime.UtcNow, "audience", null);
            Assert.That(() => new TrackOneSharedAccessSignatureToken(signature), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorValidatesTheSignatureResource()
        {
            var signature = new SettablePropertiesMock("keyName", "key", DateTime.UtcNow, "", "token!");
            Assert.That(() => new TrackOneSharedAccessSignatureToken(signature), Throws.InstanceOf<ArgumentException>());
        }

        /// <summary>
        ///   Verifies functionality of the constructor.
        /// </summary>
        ///
        [Test]
        public void ConstructorValidatesInitializesProperties()
        {
            var expiration = DateTime.UtcNow;
            var audience = "the-audience";
            var value = "TOkEn!";
            var signature = new SettablePropertiesMock("keyName", "key", expiration, audience, value);
            var token = new TrackOneSharedAccessSignatureToken(signature);

            Assert.That(token.Audience, Is.EqualTo(audience), "The audience for the token should match the signature resource.");
            Assert.That(token.TokenValue, Is.EqualTo(value), "The value for the token should match the signature value.");
            Assert.That(token.ExpiresAtUtc, Is.EqualTo(expiration), "The expiration for the token should match the signature expiration.");
            Assert.That(token.TokenType, Is.EqualTo(ClientConstants.SasTokenType), "The type for the token should match the expected constant.");
        }

        /// <summary>
        ///   Allows for the properties of the shared access signature to be manually set for
        ///   testing purposes.
        /// </summary>
        ///
        private class SettablePropertiesMock : SharedAccessSignature
        {
            public SettablePropertiesMock(string sharedAccessKeyName = default,
                                          string sharedAccessKey = default,
                                          DateTime expirationUtc = default,
                                          string resource = default,
                                          string value = default) : base()
            {
                SharedAccessKeyName = sharedAccessKeyName;
                SharedAccessKey = sharedAccessKey;
                ExpirationUtc = expirationUtc;
                Resource = resource;
                Value = value;
            }
        }
    }
}
