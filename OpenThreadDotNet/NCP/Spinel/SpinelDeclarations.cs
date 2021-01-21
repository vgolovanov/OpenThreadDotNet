namespace OpenThreadDotNet.Spinel
{
    public class SpinelEUI48
    {

        public byte[] bytes = new byte[6];
    }

    public class SpinelEUI64
    {
        public byte[] bytes = new byte[8];
    }

    public class SpinelIPv6Address
    {
        public byte[] bytes = new byte[16];
    }

    public class SpinelCommands
    {
        // Singular class that contains all Spinel constants. """

        public const byte HEADER_ASYNC = 0x80;

        public const byte HEADER_DEFAULT = 0x81;

        public const byte HEADER_EVENT_HANDLER = 0x82;

        ////=========================================
        //// Spinel Commands: Host -> NCP
        ////=========================================

        public const uint CMD_NOOP = 0;

        public const int CMD_RESET = 1;

        public const int CMD_PROP_VALUE_GET = 2;

        public const int CMD_PROP_VALUE_SET = 3;

        public const int CMD_PROP_VALUE_INSERT = 4;

        public const int CMD_PROP_VALUE_REMOVE = 5;

        ////=========================================
        //// Spinel Command Responses: NCP -> Host
        ////=========================================

        public const uint RSP_PROP_VALUE_IS = 6;

        public const uint RSP_PROP_VALUE_INSERTED = 7;

        public const uint RSP_PROP_VALUE_REMOVED = 8;      
    }

    public enum Capabilities
    {
        SPINEL_CAP_LOCK = 1,
        SPINEL_CAP_NET_SAVE = 2,
        SPINEL_CAP_HBO = 3,
        SPINEL_CAP_POWER_SAVE = 4,

        SPINEL_CAP_COUNTERS = 5,
        SPINEL_CAP_JAM_DETECT = 6,

        SPINEL_CAP_PEEK_POKE = 7,

        SPINEL_CAP_WRITABLE_RAW_STREAM = 8,
        SPINEL_CAP_GPIO = 9,
        SPINEL_CAP_TRNG = 10,
        SPINEL_CAP_CMD_MULTI = 11,
        SPINEL_CAP_UNSOL_UPDATE_FILTER = 12,
        SPINEL_CAP_MCU_POWER_STATE = 13,
        SPINEL_CAP_PCAP = 14,

        SPINEL_CAP_802_15_4__BEGIN = 16,
        SPINEL_CAP_802_15_4_2003 = (SPINEL_CAP_802_15_4__BEGIN + 0),
        SPINEL_CAP_802_15_4_2006 = (SPINEL_CAP_802_15_4__BEGIN + 1),
        SPINEL_CAP_802_15_4_2011 = (SPINEL_CAP_802_15_4__BEGIN + 2),
        SPINEL_CAP_802_15_4_PIB = (SPINEL_CAP_802_15_4__BEGIN + 5),
        SPINEL_CAP_802_15_4_2450MHZ_OQPSK = (SPINEL_CAP_802_15_4__BEGIN + 8),
        SPINEL_CAP_802_15_4_915MHZ_OQPSK = (SPINEL_CAP_802_15_4__BEGIN + 9),
        SPINEL_CAP_802_15_4_868MHZ_OQPSK = (SPINEL_CAP_802_15_4__BEGIN + 10),
        SPINEL_CAP_802_15_4_915MHZ_BPSK = (SPINEL_CAP_802_15_4__BEGIN + 11),
        SPINEL_CAP_802_15_4_868MHZ_BPSK = (SPINEL_CAP_802_15_4__BEGIN + 12),
        SPINEL_CAP_802_15_4_915MHZ_ASK = (SPINEL_CAP_802_15_4__BEGIN + 13),
        SPINEL_CAP_802_15_4_868MHZ_ASK = (SPINEL_CAP_802_15_4__BEGIN + 14),
        SPINEL_CAP_802_15_4__END = 32,

        SPINEL_CAP_CONFIG__BEGIN = 32,
        SPINEL_CAP_CONFIG_FTD = (SPINEL_CAP_CONFIG__BEGIN + 0),
        SPINEL_CAP_CONFIG_MTD = (SPINEL_CAP_CONFIG__BEGIN + 1),
        SPINEL_CAP_CONFIG_RADIO = (SPINEL_CAP_CONFIG__BEGIN + 2),
        SPINEL_CAP_CONFIG__END = 40,

        SPINEL_CAP_ROLE__BEGIN = 48,
        SPINEL_CAP_ROLE_ROUTER = (SPINEL_CAP_ROLE__BEGIN + 0),
        SPINEL_CAP_ROLE_SLEEPY = (SPINEL_CAP_ROLE__BEGIN + 1),
        SPINEL_CAP_ROLE__END = 52,

        SPINEL_CAP_NET__BEGIN = 52,
        SPINEL_CAP_NET_THREAD_1_0 = (SPINEL_CAP_NET__BEGIN + 0),
        SPINEL_CAP_NET_THREAD_1_1 = (SPINEL_CAP_NET__BEGIN + 1),
        SPINEL_CAP_NET__END = 64,

        SPINEL_CAP_OPENTHREAD__BEGIN = 512,
        SPINEL_CAP_MAC_WHITELIST = (SPINEL_CAP_OPENTHREAD__BEGIN + 0),
        SPINEL_CAP_MAC_RAW = (SPINEL_CAP_OPENTHREAD__BEGIN + 1),
        SPINEL_CAP_OOB_STEERING_DATA = (SPINEL_CAP_OPENTHREAD__BEGIN + 2),
        SPINEL_CAP_CHANNEL_MONITOR = (SPINEL_CAP_OPENTHREAD__BEGIN + 3),
        SPINEL_CAP_ERROR_RATE_TRACKING = (SPINEL_CAP_OPENTHREAD__BEGIN + 4),
        SPINEL_CAP_CHANNEL_MANAGER = (SPINEL_CAP_OPENTHREAD__BEGIN + 5),
        SPINEL_CAP_OPENTHREAD_LOG_METADATA = (SPINEL_CAP_OPENTHREAD__BEGIN + 6),
        SPINEL_CAP_TIME_SYNC = (SPINEL_CAP_OPENTHREAD__BEGIN + 7),
        SPINEL_CAP_CHILD_SUPERVISION = (SPINEL_CAP_OPENTHREAD__BEGIN + 8),
        SPINEL_CAP_POSIX_APP = (SPINEL_CAP_OPENTHREAD__BEGIN + 9),
        SPINEL_CAP_SLAAC = (SPINEL_CAP_OPENTHREAD__BEGIN + 10),
        SPINEL_CAP_OPENTHREAD__END = 640,

        SPINEL_CAP_THREAD__BEGIN = 1024,
        SPINEL_CAP_THREAD_COMMISSIONER = (SPINEL_CAP_THREAD__BEGIN + 0),
        SPINEL_CAP_THREAD_TMF_PROXY = (SPINEL_CAP_THREAD__BEGIN + 1),
        SPINEL_CAP_THREAD_UDP_FORWARD = (SPINEL_CAP_THREAD__BEGIN + 2),
        SPINEL_CAP_THREAD_JOINER = (SPINEL_CAP_THREAD__BEGIN + 3),
        SPINEL_CAP_THREAD_BORDER_ROUTER = (SPINEL_CAP_THREAD__BEGIN + 4),
        SPINEL_CAP_THREAD_SERVICE = (SPINEL_CAP_THREAD__BEGIN + 5),
        SPINEL_CAP_THREAD__END = 1152,     
    }
   
    public class SpinelProperties
    {
        ////=========================================
        //// Spinel Properties
        ////=========================================
        public const int PROP_LAST_STATUS = 0;

        public const int PROP_PROTOCOL_VERSION = 1;

        public const int PROP_NCP_VERSION = 2;

        public const int PROP_INTERFACE_TYPE = 3; // // < [i]
        public const int PROP_VENDOR_ID = 4; // < [i]
        public const int PROP_CAPS = 5;  // < capability list [A(i)]
        public const int PROP_INTERFACE_COUNT = 6; // < Interface count [C]
        public const int PROP_POWER_STATE = 7;  // < PowerState [C]

        public const int SPINEL_PROP_HOST_POWER_STATE = 12;  // < PowerState [C]
        public const int SPINEL_PROP_MCU_POWER_STATE = 13;  // < PowerState [C]

      

        public const int PROP_HWADDR = 8; // < PermEUI64 [E]
        public const int PROP_LOCK = 9; // < PropLock [b]
        public const int PROP_HBO_MEM_MAX = 10; // < Max offload mem [S]
        public const int PROP_HBO_BLOCK_MAX = 11; // < Max offload block [S]

        public const int PROP_PHY__BEGIN = 0x20;
        public const int PROP_PHY_ENABLED = PROP_PHY__BEGIN + 0; // < [b]
        public const int PROP_PHY_CHAN = PROP_PHY__BEGIN + 1; // < [C]
        public const int PROP_PHY_CHAN_SUPPORTED = PROP_PHY__BEGIN + 2;  // < [A(C)]
        public const int PROP_PHY_FREQ = PROP_PHY__BEGIN + 3;  // < kHz [L]
        public const int PROP_PHY_CCA_THRESHOLD = PROP_PHY__BEGIN + 4;  // < dBm [c]
        public const int PROP_PHY_TX_POWER = PROP_PHY__BEGIN + 5;  // < [c]
        public const int PROP_PHY_RSSI = PROP_PHY__BEGIN + 6;  // < dBm [c]
        public const int PROP_PHY__END = 0x30;

        public const int SPINEL_PROP_MAC__BEGIN = 0x30;
        public const int SPINEL_PROP_MAC_SCAN_STATE = SPINEL_PROP_MAC__BEGIN + 0;//< [C]
        public const int SPINEL_PROP_MAC_SCAN_MASK = SPINEL_PROP_MAC__BEGIN + 1;//< [A(C)]
        public const int SPINEL_PROP_MAC_SCAN_PERIOD = SPINEL_PROP_MAC__BEGIN + 2;//< ms-per-channel [S]
                                                                    //< chan,rssi,(laddr,saddr,panid,lqi),(proto,xtra) [Cct(ESSC)t(i)]
        public const int SPINEL_PROP_MAC_SCAN_BEACON = SPINEL_PROP_MAC__BEGIN + 3;
        public const int SPINEL_PROP_MAC_15_4_LADDR = SPINEL_PROP_MAC__BEGIN + 4;//< [E]
        public const int SPINEL_PROP_MAC_15_4_SADDR = SPINEL_PROP_MAC__BEGIN + 5;//< [S]
        public const int SPINEL_PROP_MAC_15_4_PANID = SPINEL_PROP_MAC__BEGIN + 6;//< [S]
        public const int SPINEL_PROP_MAC_RAW_STREAM_ENABLED = SPINEL_PROP_MAC__BEGIN + 7;//< [C]
        public const int SPINEL_PROP_MAC_FILTER_MODE = SPINEL_PROP_MAC__BEGIN + 8;//< [C]
        public const int SPINEL_PROP_MAC_ENERGY_SCAN_RESULT = SPINEL_PROP_MAC__BEGIN + 9;// `C`: Channel `c`: RSSI (in dBm)
        

        public const int PROP_MAC__END = 0x40;

        public const int SPINEL_PROP_NET__BEGIN = 0x40;
        public const int SPINEL_PROP_NET_SAVED = SPINEL_PROP_NET__BEGIN + 0;//< [b]
        public const int SPINEL_PROP_NET_IF_UP = SPINEL_PROP_NET__BEGIN + 1;//< [b]
        public const int SPINEL_PROP_NET_STACK_UP  = SPINEL_PROP_NET__BEGIN + 2;//< [C]
        public const int SPINEL_PROP_NET_ROLE = SPINEL_PROP_NET__BEGIN + 3;//< [C]
        public const int SPINEL_PROP_NET_NETWORK_NAME = SPINEL_PROP_NET__BEGIN + 4;//< [U]
        public const int SPINEL_PROP_NET_XPANID = SPINEL_PROP_NET__BEGIN + 5;//< [D]
        public const int SPINEL_PROP_NET_MASTER_KEY = SPINEL_PROP_NET__BEGIN + 6;//< [D]
        public const int SPINEL_PROP_NET_KEY_SEQUENCE_COUNTER = SPINEL_PROP_NET__BEGIN + 7;//< [L]
        public const int SPINEL_PROP_NET_PARTITION_ID = SPINEL_PROP_NET__BEGIN + 8;//< [L]
        public const int SPINEL_PROP_NET_REQUIRE_JOIN_EXISTING = SPINEL_PROP_NET__BEGIN + 9;//< [b]        
        public const int SPINEL_PROP_NET_KEY_SWITCH_GUARDTIME  = SPINEL_PROP_NET__BEGIN + 10;//< [L]
        public const int SPINEL_PROP_NET_PSKC = SPINEL_PROP_NET__BEGIN + 11;//< [D]
        public const int PROP_NET__END = 0x50;

        public const int SPINEL_PROP_THREAD__BEGIN = 0x50;
        public const int SPINEL_PROP_THREAD_LEADER_ADDR = SPINEL_PROP_THREAD__BEGIN + 0;//< [6]
        public const int SPINEL_PROP_THREAD_PARENT = SPINEL_PROP_THREAD__BEGIN + 1;//< LADDR, SADDR [ES]
        public const int SPINEL_PROP_THREAD_CHILD_TABLE = SPINEL_PROP_THREAD__BEGIN + 2;//< [A(t(ES))]
        public const int SPINEL_PROP_THREAD_LEADER_RID = SPINEL_PROP_THREAD__BEGIN + 3;//< [C]
        public const int SPINEL_PROP_THREAD_LEADER_WEIGHT = SPINEL_PROP_THREAD__BEGIN + 4;//< [C]
        public const int SPINEL_PROP_THREAD_LOCAL_LEADER_WEIGHT = SPINEL_PROP_THREAD__BEGIN + 5;//< [C]
        public const int SPINEL_PROP_THREAD_NETWORK_DATA = SPINEL_PROP_THREAD__BEGIN + 6;//< [D]
        public const int SPINEL_PROP_THREAD_NETWORK_DATA_VERSION = SPINEL_PROP_THREAD__BEGIN + 7;//< [S]
        public const int SPINEL_PROP_THREAD_STABLE_NETWORK_DATA = SPINEL_PROP_THREAD__BEGIN + 8;//< [D]
        public const int SPINEL_PROP_THREAD_STABLE_NETWORK_DATA_VERSION = SPINEL_PROP_THREAD__BEGIN + 9;//< [S]
                                                                                          //< array(ipv6prefix,prefixlen,stable,flags) [A(t(6CbC))]
        public const int SPINEL_PROP_THREAD_ON_MESH_NETS = SPINEL_PROP_THREAD__BEGIN + 10;
        //< array(ipv6prefix,prefixlen,stable,flags) [A(t(6CbC))]
        public const int SPINEL_PROP_THREAD_OFF_MESH_ROUTES = SPINEL_PROP_THREAD__BEGIN + 11;
        public const int SPINEL_PROP_THREAD_ASSISTING_PORTS = SPINEL_PROP_THREAD__BEGIN + 12;//< array(portn) [A(S)]
        public const int SPINEL_PROP_THREAD_ALLOW_LOCAL_NET_DATA_CHANGE = SPINEL_PROP_THREAD__BEGIN + 13;//< [b]
        public const int SPINEL_PROP_THREAD_MODE = SPINEL_PROP_THREAD__BEGIN + 14;

        public const int SPINEL_PROP_IPV6__BEGIN = 0x60;

        /// Link-Local IPv6 Address
        /** Format: `6` - Read only
         *
         */
        public const int SPINEL_PROP_IPV6_LL_ADDR = SPINEL_PROP_IPV6__BEGIN + 0; ///< [6]

        /// Mesh Local IPv6 Address
        /** Format: `6` - Read only
         *
         */
        public const int SPINEL_PROP_IPV6_ML_ADDR = SPINEL_PROP_IPV6__BEGIN + 1;

        /// Mesh Local Prefix
        /** Format: `6C` - Read-write
         *
         * Provides Mesh Local Prefix
         *
         *   `6`: Mesh local prefix
         *   `C` : Prefix length (64 bit for Thread).
         *
         */
        public const int SPINEL_PROP_IPV6_ML_PREFIX = SPINEL_PROP_IPV6__BEGIN + 2;

        /// IPv6 (Unicast) Address Table
        /** Format: `A(t(6CLLC))`
         *
         * This property provides all unicast addresses.
         *
         * Array of structures containing:
         *
         *  `6`: IPv6 Address
         *  `C`: Network Prefix Length
         *  `L`: Valid Lifetime
         *  `L`: Preferred Lifetime
         *
         */
        public const int SPINEL_PROP_IPV6_ADDRESS_TABLE = SPINEL_PROP_IPV6__BEGIN + 3;

        /// IPv6 Route Table - Deprecated
        public const int SPINEL_PROP_IPV6_ROUTE_TABLE = SPINEL_PROP_IPV6__BEGIN + 4;

        /// IPv6 ICMP Ping Offload
        /** Format: `b`
         *
         * Allow the NCP to directly respond to ICMP ping requests. If this is
         * turned on, ping request ICMP packets will not be passed to the host.
         *
         * Default value is `false`.
         */
        public const int SPINEL_PROP_IPV6_ICMP_PING_OFFLOAD = SPINEL_PROP_IPV6__BEGIN + 5;

        /// IPv6 Multicast Address Table
        /** Format: `A(t(6))`
         *
         * This property provides all multicast addresses.
         *
         */
        public const int SPINEL_PROP_IPV6_MULTICAST_ADDRESS_TABLE = SPINEL_PROP_IPV6__BEGIN + 6;

        /// IPv6 ICMP Ping Offload
        /** Format: `C`
         *
         * Allow the NCP to directly respond to ICMP ping requests. If this is
         * turned on, ping request ICMP packets will not be passed to the host.
         *
         * This property allows enabling responses sent to unicast only, multicast
         * only, or both. The valid value are defined by enumeration
         * `spinel_ipv6_icmp_ping_offload_mode_t`.
         *
         *   SPINEL_IPV6_ICMP_PING_OFFLOAD_DISABLED       = 0
         *   SPINEL_IPV6_ICMP_PING_OFFLOAD_UNICAST_ONLY   = 1
         *   SPINEL_IPV6_ICMP_PING_OFFLOAD_MULTICAST_ONLY = 2
         *   SPINEL_IPV6_ICMP_PING_OFFLOAD_ALL            = 3
         *
         * Default value is `NET_IPV6_ICMP_PING_OFFLOAD_DISABLED`.
         *
         */
        public const int SPINEL_PROP_IPV6_ICMP_PING_OFFLOAD_MODE = SPINEL_PROP_IPV6__BEGIN + 7; ///< [b]


        public const int PROP_STREAM__BEGIN = 0x70;
        public const int PROP_STREAM_DEBUG = PROP_STREAM__BEGIN + 0; //# < [U]
        public const int PROP_STREAM_RAW = PROP_STREAM__BEGIN + 1; // # < [D]
        public const int PROP_STREAM_NET = PROP_STREAM__BEGIN + 2; // # < [D]
        public const int PROP_STREAM_NET_INSECURE = PROP_STREAM__BEGIN + 3;//  # < [D]
        public const int PROP_STREAM__END = 0x80;

        //public const int PROP_THREAD_EXT__BEGIN = 0x1500;
        //public const int PROP_THREAD_CHILD_TIMEOUT = PROP_THREAD_EXT__BEGIN + 0;//  // < [L]
        //public const int PROP_THREAD_RLOC16 = PROP_THREAD_EXT__BEGIN + 1;//  // < [S]
        //public const int PROP_THREAD_ROUTER_UPGRADE_THRESHOLD = PROP_THREAD_EXT__BEGIN + 2;  // < [C]
        //public const int PROP_THREAD_CONTEXT_REUSE_DELAY = PROP_THREAD_EXT__BEGIN + 3;  // < [L]
        //public const int PROP_THREAD_NETWORK_ID_TIMEOUT = PROP_THREAD_EXT__BEGIN + 4;  // < [b]
        //public const int PROP_THREAD_ACTIVE_ROUTER_IDS = PROP_THREAD_EXT__BEGIN + 5;  // < [A(b)]
        //public const int PROP_THREAD_RLOC16_DEBUG_PASSTHRU = PROP_THREAD_EXT__BEGIN + 6;  // < [b]
        //public const int PROP_THREAD_ROUTER_ROLE_ENABLED = PROP_THREAD_EXT__BEGIN + 7;  // < [b]
        //public const int PROP_THREAD_ROUTER_DOWNGRADE_THRESHOLD = PROP_THREAD_EXT__BEGIN + 8;  // < [C]
        //public const int PROP_THREAD_ROUTER_SELECTION_JITTER = PROP_THREAD_EXT__BEGIN + 9;  // < [C]
        //public const int PROP_THREAD_PREFERRED_ROUTER_ID = PROP_THREAD_EXT__BEGIN + 10;  // < [C]
        //public const int PROP_THREAD_NEIGHBOR_TABLE = PROP_THREAD_EXT__BEGIN + 11;  // < [A(t(ESLCcCbLL))]
        //public const int PROP_THREAD_CHILD_COUNT_MAX = PROP_THREAD_EXT__BEGIN + 12;  // < [C]

        public const int SPINEL_PROP_THREAD_EXT__BEGIN = 0x1500;

        /// Thread Child Timeout
        /** Format: `L`
         *  Unit: Seconds
         *
         *  Used when operating in the Child role.
         */
        public const int SPINEL_PROP_THREAD_CHILD_TIMEOUT = SPINEL_PROP_THREAD_EXT__BEGIN + 0;

        /// Thread RLOC16
        /** Format: `S`
         *
         */
        public const int SPINEL_PROP_THREAD_RLOC16 = SPINEL_PROP_THREAD_EXT__BEGIN + 1;

        /// Thread Router Upgrade Threshold
        /** Format: `C`
         *
         */
        public const int SPINEL_PROP_THREAD_ROUTER_UPGRADE_THRESHOLD = SPINEL_PROP_THREAD_EXT__BEGIN + 2;

        /// Thread Context Reuse Delay
        /** Format: `L`
         *
         */
        public const int SPINEL_PROP_THREAD_CONTEXT_REUSE_DELAY = SPINEL_PROP_THREAD_EXT__BEGIN + 3;

        /// Thread Network ID Timeout
        /** Format: `C`
         *
         */
        public const int SPINEL_PROP_THREAD_NETWORK_ID_TIMEOUT = SPINEL_PROP_THREAD_EXT__BEGIN + 4;

        /// List of active thread router ids
        /** Format: `A(C)`
         *
         * Note that some implementations may not support CMD_GET_VALUE
         * router ids, but may support CMD_REMOVE_VALUE when the node is
         * a leader.
         *
         */
        public const int SPINEL_PROP_THREAD_ACTIVE_ROUTER_IDS = SPINEL_PROP_THREAD_EXT__BEGIN + 5;

        /// Forward IPv6 packets that use RLOC16 addresses to HOST.
        /** Format: `b`
         *
         * Allow host to directly observe all IPv6 packets received by the NCP,
         * including ones sent to the RLOC16 address.
         *
         * Default is false.
         *
         */
        public const int SPINEL_PROP_THREAD_RLOC16_DEBUG_PASSTHRU = SPINEL_PROP_THREAD_EXT__BEGIN + 6;

        /// Router Role Enabled
        /** Format `b`
         *
         * Allows host to indicate whether or not the router role is enabled.
         * If current role is a router, setting this property to `false` starts
         * a re-attach process as an end-device.
         *
         */
        public const int SPINEL_PROP_THREAD_ROUTER_ROLE_ENABLED = SPINEL_PROP_THREAD_EXT__BEGIN + 7;

        /// Thread Router Downgrade Threshold
        /** Format: `C`
         *
         */
        public const int SPINEL_PROP_THREAD_ROUTER_DOWNGRADE_THRESHOLD = SPINEL_PROP_THREAD_EXT__BEGIN + 8;

        /// Thread Router Selection Jitter
        /** Format: `C`
         *
         */
        public const int SPINEL_PROP_THREAD_ROUTER_SELECTION_JITTER = SPINEL_PROP_THREAD_EXT__BEGIN + 9;

        /// Thread Preferred Router Id
        /** Format: `C` - Write only
         *
         * Specifies the preferred Router Id. Upon becoming a router/leader the node
         * attempts to use this Router Id. If the preferred Router Id is not set or
         * if it can not be used, a randomly generated router id is picked. This
         * property can be set only when the device role is either detached or
         * disabled.
         *
         */
        public const int SPINEL_PROP_THREAD_PREFERRED_ROUTER_ID = SPINEL_PROP_THREAD_EXT__BEGIN + 10;

        /// Thread Neighbor Table
        /** Format: `A(t(ESLCcCbLLc))` - Read only
         *
         * Data per item is:
         *
         *  `E`: Extended address
         *  `S`: RLOC16
         *  `L`: Age (in seconds)
         *  `C`: Link Quality In
         *  `c`: Average RSS (in dBm)
         *  `C`: Mode (bit-flags)
         *  `b`: `true` if neighbor is a child, `false` otherwise.
         *  `L`: Link Frame Counter
         *  `L`: MLE Frame Counter
         *  `c`: The last RSSI (in dBm)
         *
         */
        public const int SPINEL_PROP_THREAD_NEIGHBOR_TABLE = SPINEL_PROP_THREAD_EXT__BEGIN + 11;

        /// Thread Max Child Count
        /** Format: `C`
         *
         * Specifies the maximum number of children currently allowed.
         * This parameter can only be set when Thread protocol operation
         * has been stopped.
         *
         */
        public const int SPINEL_PROP_THREAD_CHILD_COUNT_MAX = SPINEL_PROP_THREAD_EXT__BEGIN + 12;

        /// Leader Network Data
        /** Format: `D` - Read only
         *
         */
        public const int SPINEL_PROP_THREAD_LEADER_NETWORK_DATA = SPINEL_PROP_THREAD_EXT__BEGIN + 13;

        /// Stable Leader Network Data
        /** Format: `D` - Read only
         *
         */
        public const int SPINEL_PROP_THREAD_STABLE_LEADER_NETWORK_DATA = SPINEL_PROP_THREAD_EXT__BEGIN + 14;

        /// Thread Joiner Data
        /** Format `A(T(ULE))`
         *  PSKd, joiner timeout, eui64 (optional)
         *
         * This property is being deprecated by SPINEL_PROP_MESHCOP_COMMISSIONER_JOINERS.
         *
         */
        public const int SPINEL_PROP_THREAD_JOINERS = SPINEL_PROP_THREAD_EXT__BEGIN + 15;

        /// Thread Commissioner Enable
        /** Format `b`
         *
         * Default value is `false`.
         *
         * This property is being deprecated by SPINEL_PROP_MESHCOP_COMMISSIONER_STATE.
         *
         */
        public const int SPINEL_PROP_THREAD_COMMISSIONER_ENABLED = SPINEL_PROP_THREAD_EXT__BEGIN + 16;

        /// Thread TMF proxy enable
        /** Format `b`
         * Required capability: `SPINEL_CAP_THREAD_TMF_PROXY`
         *
         * This property is deprecated.
         *
         */
        public const int SPINEL_PROP_THREAD_TMF_PROXY_ENABLED = SPINEL_PROP_THREAD_EXT__BEGIN + 17;

        /// Thread TMF proxy stream
        /** Format `dSS`
         * Required capability: `SPINEL_CAP_THREAD_TMF_PROXY`
         *
         * This property is deprecated. Please see `SPINEL_PROP_THREAD_UDP_FORWARD_STREAM`.
         *
         */
        public const int SPINEL_PROP_THREAD_TMF_PROXY_STREAM = SPINEL_PROP_THREAD_EXT__BEGIN + 18;

        /// Thread "joiner" flag used during discovery scan operation
        /** Format `b`
         *
         * This property defines the Joiner Flag value in the Discovery Request TLV.
         *
         * Default value is `false`.
         *
         */
        public const int SPINEL_PROP_THREAD_DISCOVERY_SCAN_JOINER_FLAG = SPINEL_PROP_THREAD_EXT__BEGIN + 19;

        /// Enable EUI64 filtering for discovery scan operation.
        /** Format `b`
         *
         * Default value is `false`
         *
         */
        public const int SPINEL_PROP_THREAD_DISCOVERY_SCAN_ENABLE_FILTERING = SPINEL_PROP_THREAD_EXT__BEGIN + 20;

        /// PANID used for Discovery scan operation (used for PANID filtering).
        /** Format: `S`
         *
         * Default value is 0xffff (Broadcast PAN) to disable PANID filtering
         *
         */
        public const int SPINEL_PROP_THREAD_DISCOVERY_SCAN_PANID = SPINEL_PROP_THREAD_EXT__BEGIN + 21;

        /// Thread (out of band) steering data for MLE Discovery Response.
        /** Format `E` - Write only
         *
         * Required capability: SPINEL_CAP_OOB_STEERING_DATA.
         *
         * Writing to this property allows to set/update the MLE
         * Discovery Response steering data out of band.
         *
         *  - All zeros to clear the steering data (indicating that
         *    there is no steering data).
         *  - All 0xFFs to set steering data/bloom filter to
         *    accept/allow all.
         *  - A specific EUI64 which is then added to current steering
         *    data/bloom filter.
         *
         */
        public const int SPINEL_PROP_THREAD_STEERING_DATA = SPINEL_PROP_THREAD_EXT__BEGIN + 22;

        /// Thread Router Table.
        /** Format: `A(t(ESCCCCCCb)` - Read only
         *
         * Data per item is:
         *
         *  `E`: IEEE 802.15.4 Extended Address
         *  `S`: RLOC16
         *  `C`: Router ID
         *  `C`: Next hop to router
         *  `C`: Path cost to router
         *  `C`: Link Quality In
         *  `C`: Link Quality Out
         *  `C`: Age (seconds since last heard)
         *  `b`: Link established with Router ID or not.
         *
         */
        public const int SPINEL_PROP_THREAD_ROUTER_TABLE = SPINEL_PROP_THREAD_EXT__BEGIN + 23;

        /// Thread Active Operational Dataset
        /** Format: `A(t(iD))` - Read-Write
         *
         * This property provides access to current Thread Active Operational Dataset. A Thread device maintains the
         * Operational Dataset that it has stored locally and the one currently in use by the partition to which it is
         * attached. This property corresponds to the locally stored Dataset on the device.
         *
         * Operational Dataset consists of a set of supported properties (e.g., channel, master key, network name, PAN id,
         * etc). Note that not all supported properties may be present (have a value) in a Dataset.
         *
         * The Dataset value is encoded as an array of structs containing pairs of property key (as `i`) followed by the
         * property value (as `D`). The property value must follow the format associated with the corresponding property.
         *
         * On write, any unknown/unsupported property keys must be ignored.
         *
         * The following properties can be included in a Dataset list:
         *
         *   SPINEL_PROP_DATASET_ACTIVE_TIMESTAMP
         *   SPINEL_PROP_PHY_CHAN
         *   SPINEL_PROP_PHY_CHAN_SUPPORTED (Channel Mask Page 0)
         *   SPINEL_PROP_NET_MASTER_KEY
         *   SPINEL_PROP_NET_NETWORK_NAME
         *   SPINEL_PROP_NET_XPANID
         *   SPINEL_PROP_MAC_15_4_PANID
         *   SPINEL_PROP_IPV6_ML_PREFIX
         *   SPINEL_PROP_NET_PSKC
         *   SPINEL_PROP_DATASET_SECURITY_POLICY
         *
         */
        public const int SPINEL_PROP_THREAD_ACTIVE_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 24;

        /// Thread Pending Operational Dataset
        /** Format: `A(t(iD))` - Read-Write
         *
         * This property provide access to current locally stored Pending Operational Dataset.
         *
         * The formatting of this property follows the same rules as in SPINEL_PROP_THREAD_ACTIVE_DATASET.
         *
         * In addition supported properties in SPINEL_PROP_THREAD_ACTIVE_DATASET, the following properties can also
         * be included in the Pending Dataset:
         *
         *   SPINEL_PROP_DATASET_PENDING_TIMESTAMP
         *   SPINEL_PROP_DATASET_DELAY_TIMER
         *
         */
        public const int SPINEL_PROP_THREAD_PENDING_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 25;

        /// Send MGMT_SET Thread Active Operational Dataset
        /** Format: `A(t(iD))` - Write only
         *
         * The formatting of this property follows the same rules as in SPINEL_PROP_THREAD_ACTIVE_DATASET.
         *
         * This is write-only property. When written, it triggers a MGMT_ACTIVE_SET meshcop command to be sent to leader
         * with the given Dataset. The spinel frame response should be a `LAST_STATUS` with the status of the transmission
         * of MGMT_ACTIVE_SET command.
         *
         * In addition to supported properties in SPINEL_PROP_THREAD_ACTIVE_DATASET, the following property can be
         * included in the Dataset (to allow for custom raw TLVs):
         *
         *    SPINEL_PROP_DATASET_RAW_TLVS
         *
         */
        public const int SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 26;

        /// Send MGMT_SET Thread Pending Operational Dataset
        /** Format: `A(t(iD))` - Write only
         *
         * This property is similar to SPINEL_PROP_THREAD_PENDING_DATASET and follows the same format and rules.
         *
         * In addition to supported properties in SPINEL_PROP_THREAD_PENDING_DATASET, the following property can be
         * included the Dataset (to allow for custom raw TLVs to be provided).
         *
         *    SPINEL_PROP_DATASET_RAW_TLVS
         *
         */
        public const int SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 27;

        /// Operational Dataset Active Timestamp
        /** Format: `X` - No direct read or write
         *
         * It can only be included in one of the Dataset related properties below:
         *
         *   SPINEL_PROP_THREAD_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         */
        public const int SPINEL_PROP_DATASET_ACTIVE_TIMESTAMP = SPINEL_PROP_THREAD_EXT__BEGIN + 28;

        /// Operational Dataset Pending Timestamp
        /** Format: `X` - No direct read or write
         *
         * It can only be included in one of the Pending Dataset properties:
         *
         *   SPINEL_PROP_THREAD_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         */
        public const int SPINEL_PROP_DATASET_PENDING_TIMESTAMP = SPINEL_PROP_THREAD_EXT__BEGIN + 29;

        /// Operational Dataset Delay Timer
        /** Format: `L` - No direct read or write
         *
         * Delay timer (in ms) specifies the time renaming until Thread devices overwrite the value in the Active
         * Operational Dataset with the corresponding values in the Pending Operational Dataset.
         *
         * It can only be included in one of the Pending Dataset properties:
         *
         *   SPINEL_PROP_THREAD_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         */
        public const int SPINEL_PROP_DATASET_DELAY_TIMER = SPINEL_PROP_THREAD_EXT__BEGIN + 30;

        /// Operational Dataset Security Policy
        /** Format: `SC` - No direct read or write
         *
         * It can only be included in one of the Dataset related properties below:
         *
         *   SPINEL_PROP_THREAD_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         * Content is
         *   `S` : Key Rotation Time (in units of hour)
         *   `C` : Security Policy Flags (as specified in Thread 1.1 Section 8.10.1.15)
         *
         */
        public const int SPINEL_PROP_DATASET_SECURITY_POLICY = SPINEL_PROP_THREAD_EXT__BEGIN + 31;

        /// Operational Dataset Additional Raw TLVs
        /** Format: `D` - No direct read or write
         *
         * This property defines extra raw TLVs that can be added to an Operational DataSet.
         *
         * It can only be included in one of the following Dataset properties:
         *
         *   SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_SET_PENDING_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         */
        public const int SPINEL_PROP_DATASET_RAW_TLVS = SPINEL_PROP_THREAD_EXT__BEGIN + 32;

        /// Child table addresses
        /** Format: `A(t(ESA(6)))` - Read only
         *
         * This property provides the list of all addresses associated with every child
         * including any registered IPv6 addresses.
         *
         * Data per item is:
         *
         *  `E`: Extended address of the child
         *  `S`: RLOC16 of the child
         *  `A(6)`: List of IPv6 addresses registered by the child (if any)
         *
         */
        public const int SPINEL_PROP_THREAD_CHILD_TABLE_ADDRESSES = SPINEL_PROP_THREAD_EXT__BEGIN + 33;

        /// Neighbor Table Frame and Message Error Rates
        /** Format: `A(t(ESSScc))`
         *  Required capability: `CAP_ERROR_RATE_TRACKING`
         *
         * This property provides link quality related info including
         * frame and (IPv6) message error rates for all neighbors.
         *
         * With regards to message error rate, note that a larger (IPv6)
         * message can be fragmented and sent as multiple MAC frames. The
         * message transmission is considered a failure, if any of its
         * fragments fail after all MAC retry attempts.
         *
         * Data per item is:
         *
         *  `E`: Extended address of the neighbor
         *  `S`: RLOC16 of the neighbor
         *  `S`: Frame error rate (0 -> 0%, 0xffff -> 100%)
         *  `S`: Message error rate (0 -> 0%, 0xffff -> 100%)
         *  `c`: Average RSSI (in dBm)
         *  `c`: Last RSSI (in dBm)
         *
         */
        public const int SPINEL_PROP_THREAD_NEIGHBOR_TABLE_ERROR_RATES = SPINEL_PROP_THREAD_EXT__BEGIN + 34;

        /// EID (Endpoint Identifier) IPv6 Address Cache Table
        /** Format `A(t(6SC))`
         *
         * This property provides Thread EID address cache table.
         *
         * Data per item is:
         *
         *  `6` : Target IPv6 address
         *  `S` : RLOC16 of target
         *  `C` : Age (order of use, 0 indicates most recently used entry)
         *
         */
        public const int SPINEL_PROP_THREAD_ADDRESS_CACHE_TABLE = SPINEL_PROP_THREAD_EXT__BEGIN + 35;

        /// Thread UDP forward stream
        /** Format `dS6S`
         * Required capability: `SPINEL_CAP_THREAD_UDP_FORWARD`
         *
         * This property helps exchange UDP packets with host.
         *
         *  `d`: UDP payload
         *  `S`: Remote UDP port
         *  `6`: Remote IPv6 address
         *  `S`: Local UDP port
         *
         */
        public const int SPINEL_PROP_THREAD_UDP_FORWARD_STREAM = SPINEL_PROP_THREAD_EXT__BEGIN + 36;

        /// Send MGMT_GET Thread Active Operational Dataset
        /** Format: `A(t(iD))` - Write only
         *
         * The formatting of this property follows the same rules as in SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET. This
         * property further allows the sender to not include a value associated with properties in formating of `t(iD)`,
         * i.e., it should accept either a `t(iD)` or a `t(i)` encoding (in both cases indicating that the associated
         * Dataset property should be requested as part of MGMT_GET command).
         *
         * This is write-only property. When written, it triggers a MGMT_ACTIVE_GET meshcop command to be sent to leader
         * requesting the Dataset related properties from the format. The spinel frame response should be a `LAST_STATUS`
         * with the status of the transmission of MGMT_ACTIVE_GET command.
         *
         * In addition to supported properties in SPINEL_PROP_THREAD_MGMT_SET_ACTIVE_DATASET, the following property can be
         * optionally included in the Dataset:
         *
         *    SPINEL_PROP_DATASET_DEST_ADDRESS
         *
         */
        public const int SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 37;

        /// Send MGMT_GET Thread Pending Operational Dataset
        /** Format: `A(t(iD))` - Write only
         *
         * The formatting of this property follows the same rules as in SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET.
         *
         * This is write-only property. When written, it triggers a MGMT_PENDING_GET meshcop command to be sent to leader
         * with the given Dataset. The spinel frame response should be a `LAST_STATUS` with the status of the transmission
         * of MGMT_PENDING_GET command.
         *
         */
        public const int SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET = SPINEL_PROP_THREAD_EXT__BEGIN + 38;

        /// Operational Dataset (MGMT_GET) Destination IPv6 Address
        /** Format: `6` - No direct read or write
         *
         * This property specifies the IPv6 destination when sending MGMT_GET command for either Active or Pending Dataset
         * if not provided, Leader ALOC address is used as default.
         *
         * It can only be included in one of the MGMT_GET Dataset properties:
         *
         *   SPINEL_PROP_THREAD_MGMT_GET_ACTIVE_DATASET
         *   SPINEL_PROP_THREAD_MGMT_GET_PENDING_DATASET
         *
         */
        public const int SPINEL_PROP_DATASET_DEST_ADDRESS = SPINEL_PROP_THREAD_EXT__BEGIN + 39;
        public const int PROP_THREAD_EXT__END = 0x1600;

        public const int PROP_MESHCOP_EXT__BEGIN = 0x1600;
        public const int PROP_MESHCOP_JOINER_ENABLE = PROP_MESHCOP_EXT__BEGIN + 0;  // < [b]
        public const int PROP_MESHCOP_JOINER_CREDENTIAL = PROP_MESHCOP_EXT__BEGIN + 1;  // < [D]
        public const int PROP_MESHCOP_JOINER_URL = PROP_MESHCOP_EXT__BEGIN + 2;  // < [U]
        public const int PROP_MESHCOP_BORDER_AGENT_ENABLE = PROP_MESHCOP_EXT__BEGIN + 3;  // < [b]
        public const int PROP_MESHCOP_EXT__END = 0x1700;

        //public const int PROP_IPV6__BEGIN = 0x60;
        //public const int PROP_IPV6_LL_ADDR = PROP_IPV6__BEGIN + 0; // // < [6]
        //public const int PROP_IPV6_ML_ADDR = PROP_IPV6__BEGIN + 1; // // < [6C]
        //public const int PROP_IPV6_ML_PREFIX = PROP_IPV6__BEGIN + 2; // // < [6C]                                                                 
        ////// < array(ipv6addr,prefixlen,valid,preferred,flags) [A(t(6CLLC))]
        //public const int PROP_IPV6_ADDRESS_TABLE = PROP_IPV6__BEGIN + 3;
        ////// < array(ipv6prefix,prefixlen,iface,flags) [A(t(6CCC))]
        //public const int PROP_IPV6_ROUTE_TABLE = PROP_IPV6__BEGIN + 4;
        //public const int PROP_IPv6_ICMP_PING_OFFLOAD = PROP_IPV6__BEGIN + 5;//  // < [b]

        public const int SPINEL_PROP_CNTR__BEGIN = 1280;

        //// Counter reset behavior
        //// Format: `C`
        public const int SPINEL_PROP_CNTR_RESET = SPINEL_PROP_CNTR__BEGIN + 0;

        //// The total number of transmissions.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_TOTAL = PROP_CNTR__BEGIN + 1

        //// The number of transmissions with ack request.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_ACK_REQ = PROP_CNTR__BEGIN + 2

        //// The number of transmissions that were acked.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_ACKED = PROP_CNTR__BEGIN + 3

        //// The number of transmissions without ack request.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_NO_ACK_REQ = PROP_CNTR__BEGIN + 4

        //// The number of transmitted data.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_DATA = PROP_CNTR__BEGIN + 5

        //// The number of transmitted data poll.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_DATA_POLL = PROP_CNTR__BEGIN + 6

        //// The number of transmitted beacon.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_BEACON = PROP_CNTR__BEGIN + 7

        //// The number of transmitted beacon request.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_BEACON_REQ = PROP_CNTR__BEGIN + 8

        //// The number of transmitted other types of frames.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_OTHER = PROP_CNTR__BEGIN + 9

        //// The number of retransmission times.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_PKT_RETRY = PROP_CNTR__BEGIN + 10

        //// The number of CCA failure times.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_TX_ERR_CCA = PROP_CNTR__BEGIN + 11

        //// The total number of received packets.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_TOTAL = PROP_CNTR__BEGIN + 100

        //// The number of received data.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_DATA = PROP_CNTR__BEGIN + 101

        //// The number of received data poll.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_DATA_POLL = PROP_CNTR__BEGIN + 102

        //// The number of received beacon.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_BEACON = PROP_CNTR__BEGIN + 103

        //// The number of received beacon request.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_BEACON_REQ = PROP_CNTR__BEGIN + 104

        //// The number of received other types of frames.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_OTHER = PROP_CNTR__BEGIN + 105

        //// The number of received packets filtered by whitelist.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_FILT_WL = PROP_CNTR__BEGIN + 106

        //// The number of received packets filtered by destination check.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_PKT_FILT_DA = PROP_CNTR__BEGIN + 107

        //// The number of received packets that are empty.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_EMPTY = PROP_CNTR__BEGIN + 108

        //// The number of received packets from an unknown neighbor.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_UKWN_NBR = PROP_CNTR__BEGIN + 109

        //// The number of received packets whose source address is invalid.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_NVLD_SADDR = PROP_CNTR__BEGIN + 110

        //// The number of received packets with a security error.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_SECURITY = PROP_CNTR__BEGIN + 111

        //// The number of received packets with a checksum error.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_BAD_FCS = PROP_CNTR__BEGIN + 112

        //// The number of received packets with other errors.
        //// Format: `L` (Read-only) */
        //PROP_CNTR_RX_ERR_OTHER = PROP_CNTR__BEGIN + 113

        // The message buffer counter info
        // Format: `SSSSSSSSSSSSSSSS` (Read-only)
        //     `S`, (TotalBuffers)           The number of buffers in the pool.
        //     `S`, (FreeBuffers)            The number of free message buffers.
        //     `S`, (6loSendMessages)        The number of messages in the 6lo send queue.
        //     `S`, (6loSendBuffers)         The number of buffers in the 6lo send queue.
        //     `S`, (6loReassemblyMessages)  The number of messages in the 6LoWPAN reassembly queue.
        //     `S`, (6loReassemblyBuffers)   The number of buffers in the 6LoWPAN reassembly queue.
        //     `S`, (Ip6Messages)            The number of messages in the IPv6 send queue.
        //     `S`, (Ip6Buffers)             The number of buffers in the IPv6 send queue.
        //     `S`, (MplMessages)            The number of messages in the MPL send queue.
        //     `S`, (MplBuffers)             The number of buffers in the MPL send queue.
        //     `S`, (MleMessages)            The number of messages in the MLE send queue.
        //     `S`, (MleBuffers)             The number of buffers in the MLE send queue.
        //     `S`, (ArpMessages)            The number of messages in the ARP send queue.
        //     `S`, (ArpBuffers)             The number of buffers in the ARP send queue.
        //     `S`, (CoapClientMessages)     The number of messages in the CoAP client send queue.
        //     `S`  (CoapClientBuffers)      The number of buffers in the CoAP client send queue.
        public const int SPINEL_PROP_MSG_BUFFER_COUNTERS = SPINEL_PROP_CNTR__BEGIN + 400;
        public const int SPINEL_PROP_CNTR__END = 0x800;

        public const int SPINEL_PROP_OPENTHREAD__BEGIN = 0x1900;

        /// Channel Manager - Channel Change New Channel
        /** Format: `C` (read-write)
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * Setting this property triggers the Channel Manager to start
         * a channel change process. The network switches to the given
         * channel after the specified delay (see `CHANNEL_MANAGER_DELAY`).
         *
         * A subsequent write to this property will cancel an ongoing
         * (previously requested) channel change.
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_NEW_CHANNEL = SPINEL_PROP_OPENTHREAD__BEGIN + 0;

        /// Channel Manager - Channel Change Delay
        /** Format 'S'
         *  Units: seconds
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * This property specifies the delay (in seconds) to be used for
         * a channel change request.
         *
         * The delay should preferably be longer than maximum data poll
         * interval used by all sleepy-end-devices within the Thread
         * network.
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_DELAY = SPINEL_PROP_OPENTHREAD__BEGIN + 1;

        /// Channel Manager Supported Channels
        /** Format 'A(C)'
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * This property specifies the list of supported channels.
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_SUPPORTED_CHANNELS = SPINEL_PROP_OPENTHREAD__BEGIN + 2;

        /// Channel Manager Favored Channels
        /** Format 'A(C)'
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * This property specifies the list of favored channels (when `ChannelManager` is asked to select channel)
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_FAVORED_CHANNELS = SPINEL_PROP_OPENTHREAD__BEGIN + 3;

        /// Channel Manager Channel Select Trigger
        /** Format 'b'
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * Writing to this property triggers a request on `ChannelManager` to select a new channel.
         *
         * Once a Channel Select is triggered, the Channel Manager will perform the following 3 steps:
         *
         * 1) `ChannelManager` decides if the channel change would be helpful. This check can be skipped if in the input
         *    boolean to this property is set to `true` (skipping the quality check).
         *    This step uses the collected link quality metrics on the device such as CCA failure rate, frame and message
         *    error rates per neighbor, etc. to determine if the current channel quality is at the level that justifies
         *    a channel change.
         *
         * 2) If first step passes, then `ChannelManager` selects a potentially better channel. It uses the collected
         *    channel quality data by `ChannelMonitor` module. The supported and favored channels are used at this step.
         *
         * 3) If the newly selected channel is different from the current channel, `ChannelManager` requests/starts the
         *    channel change process.
         *
         * Reading this property always yields `false`.
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_CHANNEL_SELECT = SPINEL_PROP_OPENTHREAD__BEGIN + 4;

        /// Channel Manager Auto Channel Selection Enabled
        /** Format 'b'
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * This property indicates if auto-channel-selection functionality is enabled/disabled on `ChannelManager`.
         *
         * When enabled, `ChannelManager` will periodically checks and attempts to select a new channel. The period interval
         * is specified by `SPINEL_PROP_CHANNEL_MANAGER_AUTO_SELECT_INTERVAL`.
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_AUTO_SELECT_ENABLED = SPINEL_PROP_OPENTHREAD__BEGIN + 5;

        /// Channel Manager Auto Channel Selection Interval
        /** Format 'L'
         *  units: seconds
         *
         * Required capability: SPINEL_CAP_CHANNEL_MANAGER
         *
         * This property specifies the auto-channel-selection check interval (in seconds).
         *
         */
        public const int SPINEL_PROP_CHANNEL_MANAGER_AUTO_SELECT_INTERVAL = SPINEL_PROP_OPENTHREAD__BEGIN + 6;

        /// Thread network time.
        /** Format: `Xc` - Read only
         *
         * Data per item is:
         *
         *  `X`: The Thread network time, in microseconds.
         *  `c`: Time synchronization status.
         *
         */
        public const int SPINEL_PROP_THREAD_NETWORK_TIME = SPINEL_PROP_OPENTHREAD__BEGIN + 7;

        /// Thread time synchronization period
        /** Format: `S` - Read-Write
         *
         * Data per item is:
         *
         *  `S`: Time synchronization period, in seconds.
         *
         */
        public const int SPINEL_PROP_TIME_SYNC_PERIOD = SPINEL_PROP_OPENTHREAD__BEGIN + 8;

        /// Thread Time synchronization XTAL accuracy threshold for Router
        /** Format: `S` - Read-Write
         *
         * Data per item is:
         *
         *  `S`: The XTAL accuracy threshold for Router, in PPM.
         *
         */
        public const int SPINEL_PROP_TIME_SYNC_XTAL_THRESHOLD = SPINEL_PROP_OPENTHREAD__BEGIN + 9;

        /// Child Supervision Interval
        /** Format: `S` - Read-Write
         *  Units: Seconds
         *
         * Required capability: `SPINEL_CAP_CHILD_SUPERVISION`
         *
         * The child supervision interval (in seconds). Zero indicates that child supervision is disabled.
         *
         * When enabled, Child supervision feature ensures that at least one message is sent to every sleepy child within
         * the given supervision interval. If there is no other message, a supervision message (a data message with empty
         * payload) is enqueued and sent to the child.
         *
         * This property is available for FTD build only.
         *
         */
        public const int SPINEL_PROP_CHILD_SUPERVISION_INTERVAL = SPINEL_PROP_OPENTHREAD__BEGIN + 10;

        /// Child Supervision Check Timeout
        /** Format: `S` - Read-Write
         *  Units: Seconds
         *
         * Required capability: `SPINEL_CAP_CHILD_SUPERVISION`
         *
         * The child supervision check timeout interval (in seconds). Zero indicates supervision check on the child is
         * disabled.
         *
         * Supervision check is only applicable on a sleepy child. When enabled, if the child does not hear from its parent
         * within the specified check timeout, it initiates a re-attach process by starting an MLE Child Update
         * Request/Response exchange with the parent.
         *
         * This property is available for FTD and MTD builds.
         *
         */
        public const int SPINEL_PROP_CHILD_SUPERVISION_CHECK_TIMEOUT = SPINEL_PROP_OPENTHREAD__BEGIN + 11;

        // RCP (NCP in radio only mode) version
        /** Format `U` - Read only
         *
         * Required capability: SPINEL_CAP_POSIX_APP
         *
         * This property gives the version string of RCP (NCP in radio mode) which is being controlled by the POSIX
         * application. It is available only in "POSIX Application" configuration (i.e., `OPENTHREAD_ENABLE_POSIX_APP` is
         * enabled).
         *
         */
        public const int SPINEL_PROP_RCP_VERSION = SPINEL_PROP_OPENTHREAD__BEGIN + 12;

        /// Thread Parent Response info
        /** Format: `ESccCCCb` - Asynchronous event only
         *
         *  `E`: Extended address
         *  `S`: RLOC16
         *  `c`: Instant RSSI
         *  'c': Parent Priority
         *  `C`: Link Quality3
         *  `C`: Link Quality2
         *  `C`: Link Quality1
         *  'b': Is the node receiving parent response frame attached
         *
         * This property sends Parent Response frame information to the Host.
         * This property is available for FTD build only.
         *
         */
        public const int SPINEL_PROP_PARENT_RESPONSE_INFO = SPINEL_PROP_OPENTHREAD__BEGIN + 13;

        /// SLAAC enabled
        /** Format `b` - Read-Write
         *  Required capability: `SPINEL_CAP_SLAAC`
         *
         * This property allows the host to enable/disable SLAAC module on NCP at run-time. When SLAAC module is enabled,
         * SLAAC addresses (based on on-mesh prefixes in Network Data) are added to the interface. When SLAAC module is
         * disabled any previously added SLAAC address is removed.
         *
         */
        public const int SPINEL_PROP_SLAAC_ENABLED = SPINEL_PROP_OPENTHREAD__BEGIN + 14;
    }
}
