﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.Network.Models;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Network
{
    using System;
    using System.Linq;

    [Cmdlet(VerbsCommon.New, "AzureRmExpressRouteCircuitPeeringConfig", DefaultParameterSetName = "SetByResource"), OutputType(typeof(PSPeering))]
    public class NewAzureExpressRouteCircuitPeeringConfigCommand : AzureExpressRouteCircuitPeeringConfigBase
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of the Peering")]
        [ValidateNotNullOrEmpty]
        public override string Name { get; set; }

        public override void Execute()
        {
            base.Execute();

            if (string.Equals(ParameterSetName, Microsoft.Azure.Commands.Network.Properties.Resources.SetByResource))
            {
                if (this.RouteFilter != null)
                {
                    this.RouteFilterId = this.RouteFilter.Id;
                }
            }

           var peering = new PSPeering();

            peering.Name = this.Name;
            peering.PeeringType = this.PeeringType;
            peering.PrimaryPeerAddressPrefix = this.PrimaryPeerAddressPrefix;
            peering.SecondaryPeerAddressPrefix = this.SecondaryPeerAddressPrefix;
            peering.PeerASN = this.PeerASN;
            peering.VlanId = this.VlanId;

            if (!string.IsNullOrEmpty(this.SharedKey))
            {
                peering.SharedKey = this.SharedKey;
            }

            if (this.MicrosoftConfigAdvertisedPublicPrefixes != null
                && this.MicrosoftConfigAdvertisedPublicPrefixes.Any())
            {
                peering.MicrosoftPeeringConfig = new PSPeeringConfig();
                peering.MicrosoftPeeringConfig.AdvertisedPublicPrefixes = this.MicrosoftConfigAdvertisedPublicPrefixes;
                peering.MicrosoftPeeringConfig.CustomerASN = this.MicrosoftConfigCustomerAsn;
                peering.MicrosoftPeeringConfig.RoutingRegistryName = this.MicrosoftConfigRoutingRegistryName;
                peering.MicrosoftPeeringConfig.LegacyMode = Convert.ToInt32(this.LegacyMode);
            }

            if (!string.IsNullOrEmpty(this.RouteFilterId))
            {
                peering.RouteFilter = new PSRouteFilter();
                peering.RouteFilter.Id = this.RouteFilterId;
            }

            WriteObject(peering);
        }
    }
}
