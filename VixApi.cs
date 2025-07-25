using System;
using System.Runtime.InteropServices;

namespace VixBindings {
public static class VixApi {
    public const string VixDll = "Vix64AllProductsDyn.dll";

    // Basic Types
    public delegate void VixEventProc (
        int handle,
        int eventType,
        int moreEventInfo,
        IntPtr clientData );

    // Enums and Constants
    public enum VixHandleType : int
    {
        None = 0,
        Host = 2,
        VM = 3,
        Network = 5,
        Job = 6,
        Snapshot = 7,
        PropertyList = 9,
        MetadataContainer = 11
    }

    public const int VIX_INVALID_HANDLE = 0;

    public const int VIX_API_VERSION = -1;

    public enum VixError : ulong { }

    public enum VixPropertyType : int
    {
        Any = 0,
        Integer = 1,
        String = 2,
        Bool = 3,
        Handle = 4,
        Int64 = 5,
        Blob = 6
    }

    public enum VixServiceProvider : int
    {
        Default = 1,
        VMwareServer = 2,
        VMwareWorkstation = 3,
        VMwarePlayer = 4,
        VMwareVIServer = 10,
        VMwareWorkstationShared = 11
    }

    public enum VixHostOptions : int
    {
        VerifySslCert = 0x4000
    }

    public enum VixVMOpenOptions : int
    {
        Normal = 0x0
    }

    public enum VixVMPowerOpOptions : int
    {
        Normal = 0,
        FromGuest = 0x0004,
        SuppressSnapshotPowerOn = 0x0080,
        LaunchGui = 0x0200,
        StartVmPaused = 0x1000
    }

    public enum VixVMDeleteOptions : int
    {
        DiskFiles = 0x0002
    }

    public enum VixPowerState : int
    {
        PoweringOff = 0x0001,
        PoweredOff = 0x0002,
        PoweringOn = 0x0004,
        PoweredOn = 0x0008,
        Suspending = 0x0010,
        Suspended = 0x0020,
        ToolsRunning = 0x0040,
        Resetting = 0x0080,
        BlockedOnMsg = 0x0100,
        Paused = 0x0200,
        Resuming = 0x0800
    }

    public enum VixToolsState : int
    {
        Unknown = 0x0001,
        Running = 0x0002,
        NotInstalled = 0x0004
    }

    public enum VixRunProgramOptions : int
    {
        ReturnImmediately = 0x0001,
        ActivateWindow = 0x0002
    }

    public enum VixRemoveSnapshotOptions : int
    {
        RemoveChildren = 0x0001
    }

    public enum VixCreateSnapshotOptions : int
    {
        IncludeMemory = 0x0002
    }

    public enum VixMsgSharedFolderOptions : int
    {
        WriteAccess = 0x04
    }

    public enum VixCloneType : int
    {
        Full = 0,
        Linked = 1
    }

    public enum VixCaptureScreenFormat : int
    {
        Png = 0x01,
        PngNoCompress = 0x02
    }

    public enum VixInstallToolsOptions : int
    {
        MountToolsInstaller = 0x00,
        AutoUpgrade = 0x01,
        ReturnImmediately = 0x02
    }

    // Property IDs
    public const int VIX_PROPERTY_NONE = 0;
    public const int VIX_PROPERTY_META_DATA_CONTAINER = 2;
    public const int VIX_PROPERTY_HOST_HOSTTYPE = 50;
    public const int VIX_PROPERTY_HOST_API_VERSION = 51;
    public const int VIX_PROPERTY_HOST_SOFTWARE_VERSION = 52;
    public const int VIX_PROPERTY_VM_NUM_VCPUS = 101;
    public const int VIX_PROPERTY_VM_VMX_PATHNAME = 103;
    public const int VIX_PROPERTY_VM_VMTEAM_PATHNAME = 105;
    public const int VIX_PROPERTY_VM_MEMORY_SIZE = 106;
    public const int VIX_PROPERTY_VM_READ_ONLY = 107;
    public const int VIX_PROPERTY_VM_NAME = 108;
    public const int VIX_PROPERTY_VM_GUESTOS = 109;
    public const int VIX_PROPERTY_VM_IN_VMTEAM = 128;
    public const int VIX_PROPERTY_VM_POWER_STATE = 129;
    public const int VIX_PROPERTY_VM_TOOLS_STATE = 152;
    public const int VIX_PROPERTY_VM_IS_RUNNING = 196;
    public const int VIX_PROPERTY_VM_SUPPORTED_FEATURES = 197;
    public const int VIX_PROPERTY_VM_SSL_ERROR = 293;
    public const int VIX_PROPERTY_JOB_RESULT_ERROR_CODE = 3000;
    public const int VIX_PROPERTY_JOB_RESULT_VM_IN_GROUP = 3001;
    public const int VIX_PROPERTY_JOB_RESULT_USER_MESSAGE = 3002;
    public const int VIX_PROPERTY_JOB_RESULT_EXIT_CODE = 3004;
    public const int VIX_PROPERTY_JOB_RESULT_COMMAND_OUTPUT = 3005;
    public const int VIX_PROPERTY_JOB_RESULT_HANDLE = 3010;
    public const int VIX_PROPERTY_JOB_RESULT_GUEST_OBJECT_EXISTS = 3011;
    public const int VIX_PROPERTY_JOB_RESULT_GUEST_PROGRAM_ELAPSED_TIME = 3017;
    public const int VIX_PROPERTY_JOB_RESULT_GUEST_PROGRAM_EXIT_CODE = 3018;
    public const int VIX_PROPERTY_JOB_RESULT_ITEM_NAME = 3035;
    public const int VIX_PROPERTY_JOB_RESULT_FOUND_ITEM_DESCRIPTION = 3036;
    public const int VIX_PROPERTY_JOB_RESULT_SHARED_FOLDER_COUNT = 3046;
    public const int VIX_PROPERTY_JOB_RESULT_SHARED_FOLDER_HOST = 3048;
    public const int VIX_PROPERTY_JOB_RESULT_SHARED_FOLDER_FLAGS = 3049;
    public const int VIX_PROPERTY_JOB_RESULT_PROCESS_ID = 3051;
    public const int VIX_PROPERTY_JOB_RESULT_PROCESS_OWNER = 3052;
    public const int VIX_PROPERTY_JOB_RESULT_PROCESS_COMMAND = 3053;
    public const int VIX_PROPERTY_JOB_RESULT_FILE_FLAGS = 3054;
    public const int VIX_PROPERTY_JOB_RESULT_PROCESS_START_TIME = 3055;
    public const int VIX_PROPERTY_JOB_RESULT_VM_VARIABLE_STRING = 3056;
    public const int VIX_PROPERTY_JOB_RESULT_PROCESS_BEING_DEBUGGED = 3057;
    public const int VIX_PROPERTY_JOB_RESULT_SCREEN_IMAGE_SIZE = 3058;
    public const int VIX_PROPERTY_JOB_RESULT_SCREEN_IMAGE_DATA = 3059;
    public const int VIX_PROPERTY_JOB_RESULT_FILE_SIZE = 3061;
    public const int VIX_PROPERTY_JOB_RESULT_FILE_MOD_TIME = 3062;
    public const int VIX_PROPERTY_JOB_RESULT_EXTRA_ERROR_INFO = 3084;
    public const int VIX_PROPERTY_FOUND_ITEM_LOCATION = 4010;
    public const int VIX_PROPERTY_SNAPSHOT_DISPLAYNAME = 4200;
    public const int VIX_PROPERTY_SNAPSHOT_DESCRIPTION = 4201;
    public const int VIX_PROPERTY_SNAPSHOT_POWERSTATE = 4205;
    public const int VIX_PROPERTY_GUEST_SHAREDFOLDERS_SHARES_PATH = 4525;
    public const int VIX_PROPERTY_VM_ENCRYPTION_PASSWORD = 7001;

    // Error codes (полный список)
    public const int VIX_OK = 0;
    public const int VIX_E_FAIL = 1;
    public const int VIX_E_OUT_OF_MEMORY = 2;
    public const int VIX_E_INVALID_ARG = 3;
    public const int VIX_E_FILE_NOT_FOUND = 4;
    public const int VIX_E_OBJECT_IS_BUSY = 5;
    public const int VIX_E_NOT_SUPPORTED = 6;
    public const int VIX_E_FILE_ERROR = 7;
    public const int VIX_E_DISK_FULL = 8;
    public const int VIX_E_INCORRECT_FILE_TYPE = 9;
    public const int VIX_E_CANCELLED = 10;
    public const int VIX_E_FILE_READ_ONLY = 11;
    public const int VIX_E_FILE_ALREADY_EXISTS = 12;
    public const int VIX_E_FILE_ACCESS_ERROR = 13;
    public const int VIX_E_REQUIRES_LARGE_FILES = 14;
    public const int VIX_E_FILE_ALREADY_LOCKED = 15;
    public const int VIX_E_VMDB = 16;
    public const int VIX_E_NOT_SUPPORTED_ON_REMOTE_OBJECT = 20;
    public const int VIX_E_FILE_TOO_BIG = 21;
    public const int VIX_E_FILE_NAME_INVALID = 22;
    public const int VIX_E_ALREADY_EXISTS = 23;
    public const int VIX_E_BUFFER_TOOSMALL = 24;
    public const int VIX_E_OBJECT_NOT_FOUND = 25;
    public const int VIX_E_HOST_NOT_CONNECTED = 26;
    public const int VIX_E_INVALID_UTF8_STRING = 27;
    public const int VIX_E_OPERATION_ALREADY_IN_PROGRESS = 31;
    public const int VIX_E_UNFINISHED_JOB = 29;
    public const int VIX_E_NEED_KEY = 30;
    public const int VIX_E_LICENSE = 32;
    public const int VIX_E_VM_HOST_DISCONNECTED = 34;
    public const int VIX_E_AUTHENTICATION_FAIL = 35;
    public const int VIX_E_HOST_CONNECTION_LOST = 36;
    public const int VIX_E_DUPLICATE_NAME = 41;
    public const int VIX_E_ARGUMENT_TOO_BIG = 44;
    public const int VIX_E_INVALID_HANDLE = 1000;
    public const int VIX_E_NOT_SUPPORTED_ON_HANDLE_TYPE = 1001;
    public const int VIX_E_TOO_MANY_HANDLES = 1002;
    public const int VIX_E_NOT_FOUND = 2000;
    public const int VIX_E_TYPE_MISMATCH = 2001;
    public const int VIX_E_INVALID_XML = 2002;
    public const int VIX_E_TIMEOUT_WAITING_FOR_TOOLS = 3000;
    public const int VIX_E_UNRECOGNIZED_COMMAND = 3001;
    public const int VIX_E_OP_NOT_SUPPORTED_ON_GUEST = 3003;
    public const int VIX_E_PROGRAM_NOT_STARTED = 3004;
    public const int VIX_E_CANNOT_START_READ_ONLY_VM = 3005;
    public const int VIX_E_VM_NOT_RUNNING = 3006;
    public const int VIX_E_VM_IS_RUNNING = 3007;
    public const int VIX_E_CANNOT_CONNECT_TO_VM = 3008;
    public const int VIX_E_POWEROP_SCRIPTS_NOT_AVAILABLE = 3009;
    public const int VIX_E_NO_GUEST_OS_INSTALLED = 3010;
    public const int VIX_E_VM_INSUFFICIENT_HOST_MEMORY = 3011;
    public const int VIX_E_SUSPEND_ERROR = 3012;
    public const int VIX_E_VM_NOT_ENOUGH_CPUS = 3013;
    public const int VIX_E_HOST_USER_PERMISSIONS = 3014;
    public const int VIX_E_GUEST_USER_PERMISSIONS = 3015;
    public const int VIX_E_TOOLS_NOT_RUNNING = 3016;
    public const int VIX_E_GUEST_OPERATIONS_PROHIBITED = 3017;
    public const int VIX_E_ANON_GUEST_OPERATIONS_PROHIBITED = 3018;
    public const int VIX_E_ROOT_GUEST_OPERATIONS_PROHIBITED = 3019;
    public const int VIX_E_MISSING_ANON_GUEST_ACCOUNT = 3023;
    public const int VIX_E_CANNOT_AUTHENTICATE_WITH_GUEST = 3024;
    public const int VIX_E_UNRECOGNIZED_COMMAND_IN_GUEST = 3025;
    public const int VIX_E_CONSOLE_GUEST_OPERATIONS_PROHIBITED = 3026;
    public const int VIX_E_MUST_BE_CONSOLE_USER = 3027;
    public const int VIX_E_VMX_MSG_DIALOG_AND_NO_UI = 3028;
    public const int VIX_E_OPERATION_NOT_ALLOWED_FOR_LOGIN_TYPE = 3031;
    public const int VIX_E_LOGIN_TYPE_NOT_SUPPORTED = 3032;
    public const int VIX_E_EMPTY_PASSWORD_NOT_ALLOWED_IN_GUEST = 3033;
    public const int VIX_E_INTERACTIVE_SESSION_NOT_PRESENT = 3034;
    public const int VIX_E_INTERACTIVE_SESSION_USER_MISMATCH = 3035;
    public const int VIX_E_CANNOT_POWER_ON_VM = 3041;
    public const int VIX_E_NO_DISPLAY_SERVER = 3043;
    public const int VIX_E_TOO_MANY_LOGONS = 3046;
    public const int VIX_E_INVALID_AUTHENTICATION_SESSION = 3047;
    public const int VIX_E_VM_NOT_FOUND = 4000;
    public const int VIX_E_NOT_SUPPORTED_FOR_VM_VERSION = 4001;
    public const int VIX_E_CANNOT_READ_VM_CONFIG = 4002;
    public const int VIX_E_TEMPLATE_VM = 4003;
    public const int VIX_E_VM_ALREADY_LOADED = 4004;
    public const int VIX_E_VM_ALREADY_UP_TO_DATE = 4006;
    public const int VIX_E_VM_UNSUPPORTED_GUEST = 4011;
    public const int VIX_E_UNRECOGNIZED_PROPERTY = 6000;
    public const int VIX_E_INVALID_PROPERTY_VALUE = 6001;
    public const int VIX_E_READ_ONLY_PROPERTY = 6002;
    public const int VIX_E_MISSING_REQUIRED_PROPERTY = 6003;
    public const int VIX_E_INVALID_SERIALIZED_DATA = 6004;
    public const int VIX_E_PROPERTY_TYPE_MISMATCH = 6005;
    public const int VIX_E_BAD_VM_INDEX = 8000;
    public const int VIX_E_INVALID_MESSAGE_HEADER = 10000;
    public const int VIX_E_INVALID_MESSAGE_BODY = 10001;
    public const int VIX_E_SNAPSHOT_INVAL = 13000;
    public const int VIX_E_SNAPSHOT_DUMPER = 13001;
    public const int VIX_E_SNAPSHOT_DISKLIB = 13002;
    public const int VIX_E_SNAPSHOT_NOTFOUND = 13003;
    public const int VIX_E_SNAPSHOT_EXISTS = 13004;
    public const int VIX_E_SNAPSHOT_VERSION = 13005;
    public const int VIX_E_SNAPSHOT_NOPERM = 13006;
    public const int VIX_E_SNAPSHOT_CONFIG = 13007;
    public const int VIX_E_SNAPSHOT_NOCHANGE = 13008;
    public const int VIX_E_SNAPSHOT_CHECKPOINT = 13009;
    public const int VIX_E_SNAPSHOT_LOCKED = 13010;
    public const int VIX_E_SNAPSHOT_INCONSISTENT = 13011;
    public const int VIX_E_SNAPSHOT_NAMETOOLONG = 13012;
    public const int VIX_E_SNAPSHOT_VIXFILE = 13013;
    public const int VIX_E_SNAPSHOT_DISKLOCKED = 13014;
    public const int VIX_E_SNAPSHOT_DUPLICATEDDISK = 13015;
    public const int VIX_E_SNAPSHOT_INDEPENDENTDISK = 13016;
    public const int VIX_E_SNAPSHOT_NONUNIQUE_NAME = 13017;
    public const int VIX_E_SNAPSHOT_MEMORY_ON_INDEPENDENT_DISK = 13018;
    public const int VIX_E_SNAPSHOT_MAXSNAPSHOTS = 13019;
    public const int VIX_E_SNAPSHOT_MIN_FREE_SPACE = 13020;
    public const int VIX_E_SNAPSHOT_HIERARCHY_TOODEEP = 13021;
    public const int VIX_E_SNAPSHOT_NOT_REVERTABLE = 13024;
    public const int VIX_E_HOST_DISK_INVALID_VALUE = 14003;
    public const int VIX_E_HOST_DISK_SECTORSIZE = 14004;
    public const int VIX_E_HOST_FILE_ERROR_EOF = 14005;
    public const int VIX_E_HOST_NETBLKDEV_HANDSHAKE = 14006;
    public const int VIX_E_HOST_SOCKET_CREATION_ERROR = 14007;
    public const int VIX_E_HOST_SERVER_NOT_FOUND = 14008;
    public const int VIX_E_HOST_NETWORK_CONN_REFUSED = 14009;
    public const int VIX_E_HOST_TCP_SOCKET_ERROR = 14010;
    public const int VIX_E_HOST_TCP_CONN_LOST = 14011;
    public const int VIX_E_HOST_NBD_HASHFILE_VOLUME = 14012;
    public const int VIX_E_HOST_NBD_HASHFILE_INIT = 14013;
    public const int VIX_E_HOST_SERVER_SHUTDOWN = 14014;
    public const int VIX_E_HOST_SERVER_NOT_AVAILABLE = 14015;
    public const int VIX_E_DISK_INVAL = 16000;
    public const int VIX_E_DISK_NOINIT = 16001;
    public const int VIX_E_DISK_NOIO = 16002;
    public const int VIX_E_DISK_PARTIALCHAIN = 16003;
    public const int VIX_E_DISK_NEEDSREPAIR = 16006;
    public const int VIX_E_DISK_OUTOFRANGE = 16007;
    public const int VIX_E_DISK_CID_MISMATCH = 16008;
    public const int VIX_E_DISK_CANTSHRINK = 16009;
    public const int VIX_E_DISK_PARTMISMATCH = 16010;
    public const int VIX_E_DISK_UNSUPPORTEDDISKVERSION = 16011;
    public const int VIX_E_DISK_OPENPARENT = 16012;
    public const int VIX_E_DISK_NOTSUPPORTED = 16013;
    public const int VIX_E_DISK_NEEDKEY = 16014;
    public const int VIX_E_DISK_NOKEYOVERRIDE = 16015;
    public const int VIX_E_DISK_NOTENCRYPTED = 16016;
    public const int VIX_E_DISK_NOKEY = 16017;
    public const int VIX_E_DISK_INVALIDPARTITIONTABLE = 16018;
    public const int VIX_E_DISK_NOTNORMAL = 16019;
    public const int VIX_E_DISK_NOTENCDESC = 16020;
    public const int VIX_E_DISK_NEEDVMFS = 16022;
    public const int VIX_E_DISK_RAWTOOBIG = 16024;
    public const int VIX_E_DISK_TOOMANYOPENFILES = 16027;
    public const int VIX_E_DISK_TOOMANYREDO = 16028;
    public const int VIX_E_DISK_RAWTOOSMALL = 16029;
    public const int VIX_E_DISK_INVALIDCHAIN = 16030;
    public const int VIX_E_DISK_KEY_NOTFOUND = 16052;
    public const int VIX_E_DISK_SUBSYSTEM_INIT_FAIL = 16053;
    public const int VIX_E_DISK_INVALID_CONNECTION = 16054;
    public const int VIX_E_DISK_ENCODING = 16061;
    public const int VIX_E_DISK_CANTREPAIR = 16062;
    public const int VIX_E_DISK_INVALIDDISK = 16063;
    public const int VIX_E_DISK_NOLICENSE = 16064;
    public const int VIX_E_DISK_NODEVICE = 16065;
    public const int VIX_E_DISK_UNSUPPORTEDDEVICE = 16066;
    public const int VIX_E_DISK_CAPACITY_MISMATCH = 16067;
    public const int VIX_E_DISK_PARENT_NOTALLOWED = 16068;
    public const int VIX_E_DISK_ATTACH_ROOTLINK = 16069;
    public const int VIX_E_CRYPTO_UNKNOWN_ALGORITHM = 17000;
    public const int VIX_E_CRYPTO_BAD_BUFFER_SIZE = 17001;
    public const int VIX_E_CRYPTO_INVALID_OPERATION = 17002;
    public const int VIX_E_CRYPTO_RANDOM_DEVICE = 17003;
    public const int VIX_E_CRYPTO_NEED_PASSWORD = 17004;
    public const int VIX_E_CRYPTO_BAD_PASSWORD = 17005;
    public const int VIX_E_CRYPTO_NOT_IN_DICTIONARY = 17006;
    public const int VIX_E_CRYPTO_NO_CRYPTO = 17007;
    public const int VIX_E_CRYPTO_ERROR = 17008;
    public const int VIX_E_CRYPTO_BAD_FORMAT = 17009;
    public const int VIX_E_CRYPTO_LOCKED = 17010;
    public const int VIX_E_CRYPTO_EMPTY = 17011;
    public const int VIX_E_CRYPTO_KEYSAFE_LOCATOR = 17012;
    public const int VIX_E_CANNOT_CONNECT_TO_HOST = 18000;
    public const int VIX_E_NOT_FOR_REMOTE_HOST = 18001;
    public const int VIX_E_INVALID_HOSTNAME_SPECIFICATION = 18002;
    public const int VIX_E_SCREEN_CAPTURE_ERROR = 19000;
    public const int VIX_E_SCREEN_CAPTURE_BAD_FORMAT = 19001;
    public const int VIX_E_SCREEN_CAPTURE_COMPRESSION_FAIL = 19002;
    public const int VIX_E_SCREEN_CAPTURE_LARGE_DATA = 19003;
    public const int VIX_E_GUEST_VOLUMES_NOT_FROZEN = 20000;
    public const int VIX_E_NOT_A_FILE = 20001;
    public const int VIX_E_NOT_A_DIRECTORY = 20002;
    public const int VIX_E_NO_SUCH_PROCESS = 20003;
    public const int VIX_E_FILE_NAME_TOO_LONG = 20004;
    public const int VIX_E_OPERATION_DISABLED = 20005;
    public const int VIX_E_TOOLS_INSTALL_NO_IMAGE = 21000;
    public const int VIX_E_TOOLS_INSTALL_IMAGE_INACCESIBLE = 21001;
    public const int VIX_E_TOOLS_INSTALL_NO_DEVICE = 21002;
    public const int VIX_E_TOOLS_INSTALL_DEVICE_NOT_CONNECTED = 21003;
    public const int VIX_E_TOOLS_INSTALL_CANCELLED = 21004;
    public const int VIX_E_TOOLS_INSTALL_INIT_FAILED = 21005;
    public const int VIX_E_TOOLS_INSTALL_AUTO_NOT_SUPPORTED = 21006;
    public const int VIX_E_TOOLS_INSTALL_GUEST_NOT_READY = 21007;
    public const int VIX_E_TOOLS_INSTALL_SIG_CHECK_FAILED = 21008;
    public const int VIX_E_TOOLS_INSTALL_ERROR = 21009;
    public const int VIX_E_TOOLS_INSTALL_ALREADY_UP_TO_DATE = 21010;
    public const int VIX_E_TOOLS_INSTALL_IN_PROGRESS = 21011;
    public const int VIX_E_TOOLS_INSTALL_IMAGE_COPY_FAILED = 21012;
    public const int VIX_E_WRAPPER_WORKSTATION_NOT_INSTALLED = 22001;
    public const int VIX_E_WRAPPER_VERSION_NOT_FOUND = 22002;
    public const int VIX_E_WRAPPER_SERVICEPROVIDER_NOT_FOUND = 22003;
    public const int VIX_E_WRAPPER_PLAYER_NOT_INSTALLED = 22004;
    public const int VIX_E_WRAPPER_RUNTIME_NOT_INSTALLED = 22005;
    public const int VIX_E_WRAPPER_MULTIPLE_SERVICEPROVIDERS = 22006;
    public const int VIX_E_MNTAPI_MOUNTPT_NOT_FOUND = 24000;
    public const int VIX_E_MNTAPI_MOUNTPT_IN_USE = 24001;
    public const int VIX_E_MNTAPI_DISK_NOT_FOUND = 24002;
    public const int VIX_E_MNTAPI_DISK_NOT_MOUNTED = 24003;
    public const int VIX_E_MNTAPI_DISK_IS_MOUNTED = 24004;
    public const int VIX_E_MNTAPI_DISK_NOT_SAFE = 24005;
    public const int VIX_E_MNTAPI_DISK_CANT_OPEN = 24006;
    public const int VIX_E_MNTAPI_CANT_READ_PARTS = 24007;
    public const int VIX_E_MNTAPI_UMOUNT_APP_NOT_FOUND = 24008;
    public const int VIX_E_MNTAPI_UMOUNT = 24009;
    public const int VIX_E_MNTAPI_NO_MOUNTABLE_PARTITONS = 24010;
    public const int VIX_E_MNTAPI_PARTITION_RANGE = 24011;
    public const int VIX_E_MNTAPI_PERM = 24012;
    public const int VIX_E_MNTAPI_DICT = 24013;
    public const int VIX_E_MNTAPI_DICT_LOCKED = 24014;
    public const int VIX_E_MNTAPI_OPEN_HANDLES = 24015;
    public const int VIX_E_MNTAPI_CANT_MAKE_VAR_DIR = 24016;
    public const int VIX_E_MNTAPI_NO_ROOT = 24017;
    public const int VIX_E_MNTAPI_LOOP_FAILED = 24018;
    public const int VIX_E_MNTAPI_DAEMON = 24019;
    public const int VIX_E_MNTAPI_INTERNAL = 24020;
    public const int VIX_E_MNTAPI_SYSTEM = 24021;
    public const int VIX_E_MNTAPI_NO_CONNECTION_DETAILS = 24022;
    public const int VIX_E_MNTAPI_INCOMPATIBLE_VERSION = 24300;
    public const int VIX_E_MNTAPI_OS_ERROR = 24301;
    public const int VIX_E_MNTAPI_DRIVE_LETTER_IN_USE = 24302;
    public const int VIX_E_MNTAPI_DRIVE_LETTER_ALREADY_ASSIGNED = 24303;
    public const int VIX_E_MNTAPI_VOLUME_NOT_MOUNTED = 24304;
    public const int VIX_E_MNTAPI_VOLUME_ALREADY_MOUNTED = 24305;
    public const int VIX_E_MNTAPI_FORMAT_FAILURE = 24306;
    public const int VIX_E_MNTAPI_NO_DRIVER = 24307;
    public const int VIX_E_MNTAPI_ALREADY_OPENED = 24308;
    public const int VIX_E_MNTAPI_ITEM_NOT_FOUND = 24309;
    public const int VIX_E_UNSUPPROTED_BOOT_LOADER = 24310;
    public const int VIX_E_UNSUPPROTED_OS = 24311;
    public const int VIX_E_CODECONVERSION = 24312;
    public const int VIX_E_REGWRITE_ERROR = 24313;
    public const int VIX_E_UNSUPPORTED_FT_VOLUME = 24314;
    public const int VIX_E_PARTITION_NOT_FOUND = 24315;
    public const int VIX_E_PUTFILE_ERROR = 24316;
    public const int VIX_E_GETFILE_ERROR = 24317;
    public const int VIX_E_REG_NOT_OPENED = 24318;
    public const int VIX_E_REGDELKEY_ERROR = 24319;
    public const int VIX_E_CREATE_PARTITIONTABLE_ERROR = 24320;
    public const int VIX_E_OPEN_FAILURE = 24321;
    public const int VIX_E_VOLUME_NOT_WRITABLE = 24322;
    public const int VIX_ASYNC = 25000;
    public const int VIX_E_ASYNC_MIXEDMODE_UNSUPPORTED = 26000;
    public const int VIX_E_NET_HTTP_UNSUPPORTED_PROTOCOL = 30001;
    public const int VIX_E_NET_HTTP_URL_MALFORMAT = 30003;
    public const int VIX_E_NET_HTTP_COULDNT_RESOLVE_PROXY = 30005;
    public const int VIX_E_NET_HTTP_COULDNT_RESOLVE_HOST = 30006;
    public const int VIX_E_NET_HTTP_COULDNT_CONNECT = 30007;
    public const int VIX_E_NET_HTTP_HTTP_RETURNED_ERROR = 30022;
    public const int VIX_E_NET_HTTP_OPERATION_TIMEDOUT = 30028;
    public const int VIX_E_NET_HTTP_SSL_CONNECT_ERROR = 30035;
    public const int VIX_E_NET_HTTP_TOO_MANY_REDIRECTS = 30047;
    public const int VIX_E_NET_HTTP_TRANSFER = 30200;
    public const int VIX_E_NET_HTTP_SSL_SECURITY = 30201;
    public const int VIX_E_NET_HTTP_GENERIC = 30202;

    public enum VixEventType : int
    {
        JobCompleted = 2,
        JobProgress = 3,
        FindItem = 8,
        CallbackSignalled = 2 // Deprecated - Use JobCompleted instead.
    }

    public const int VIX_FILE_ATTRIBUTES_DIRECTORY = 0x0001;
    public const int VIX_FILE_ATTRIBUTES_SYMLINK = 0x0002;

    public enum VixFindItemType : int
    {
        RunningVms = 1,
        RegisteredVms = 4
    }

    public const int VIX_VM_SUPPORT_SHARED_FOLDERS = 0x0001;
    public const int VIX_VM_SUPPORT_MULTIPLE_SNAPSHOTS = 0x0002;
    public const int VIX_VM_SUPPORT_TOOLS_INSTALL = 0x0004;
    public const int VIX_VM_SUPPORT_HARDWARE_UPGRADE = 0x0008;

    public const int VIX_LOGIN_IN_GUEST_REQUIRE_INTERACTIVE_ENVIRONMENT = 0x08;

    public const int VIX_VM_GUEST_VARIABLE = 1;
    public const int VIX_VM_CONFIG_RUNTIME_ONLY = 2;
    public const int VIX_GUEST_ENVIRONMENT_VARIABLE = 3;

    // Main API Functions
    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern IntPtr Vix_GetErrorText ( ulong err, string? locale );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern ulong VixPropertyList_AllocPropertyList ( int hostHandle, out int resultHandle, int firstPropertyID /*, ...*/ );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern void Vix_ReleaseHandle ( int handle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern void Vix_AddRefHandle ( int handle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern int Vix_GetHandleType ( int handle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong Vix_GetProperties ( int handle, int firstPropertyID /*, ...*/ );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong Vix_GetProperties ( int handle, int propertyId, out IntPtr value, int terminator );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern ulong Vix_GetPropertyType ( int handle, int propertyID, out int propertyType );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern void Vix_FreeBuffer ( IntPtr p );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixHost_Connect (
        int apiVersion,
        VixServiceProvider hostType,
        string hostName,
        int hostPort,
        string userName,
        string password,
        VixHostOptions options,
        int propertyListHandle,
        VixEventProc? callbackProc,
        IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern void VixHost_Disconnect ( int hostHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixHost_RegisterVM ( int hostHandle, string vmxFilePath, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixHost_UnregisterVM ( int hostHandle, string vmxFilePath, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl )]
    public static extern int VixHost_FindItems ( int hostHandle, int searchType, int searchCriteria, int timeout, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixHost_OpenVM ( int hostHandle, string vmxFilePathName, VixVMOpenOptions options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Open ( int hostHandle, string vmxFilePathName, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_PowerOn ( int vmHandle, VixVMPowerOpOptions powerOnOptions, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_PowerOff ( int vmHandle, VixVMPowerOpOptions powerOffOptions, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Reset ( int vmHandle, VixVMPowerOpOptions resetOptions, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Suspend ( int vmHandle, VixVMPowerOpOptions suspendOptions, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Pause ( int vmHandle, int options, int propertyList, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Unpause ( int vmHandle, int options, int propertyList, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Delete ( int vmHandle, VixVMDeleteOptions deleteOptions, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_WaitForToolsInGuest ( int vmHandle, int timeoutInSeconds, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_LoginInGuest ( int vmHandle, string userName, string password, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_LogoutFromGuest ( int vmHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RunProgramInGuest ( int vmHandle, string guestProgramName, string commandLineArgs, VixRunProgramOptions options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_ListProcessesInGuest ( int vmHandle, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_KillProcessInGuest ( int vmHandle, ulong pid, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RunScriptInGuest ( int vmHandle, string interpreter, string scriptText, VixRunProgramOptions options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CopyFileFromHostToGuest ( int vmHandle, string hostPathName, string guestPathName, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CopyFileFromGuestToHost ( int vmHandle, string guestPathName, string hostPathName, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_DeleteFileInGuest ( int vmHandle, string guestPathName, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_FileExistsInGuest ( int vmHandle, string guestPathName, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RenameFileInGuest ( int vmHandle, string oldName, string newName, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CreateTempFileInGuest ( int vmHandle, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_GetFileInfoInGuest ( int vmHandle, string pathName, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_ListDirectoryInGuest ( int vmHandle, string pathName, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CreateDirectoryInGuest ( int vmHandle, string pathName, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_DeleteDirectoryInGuest ( int vmHandle, string pathName, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_DirectoryExistsInGuest ( int vmHandle, string pathName, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_ReadVariable ( int vmHandle, int variableType, string name, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_WriteVariable ( int vmHandle, int variableType, string valueName, string value, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixVM_GetNumRootSnapshots ( int vmHandle, out int result );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixVM_GetRootSnapshot ( int vmHandle, int index, out int snapshotHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixVM_GetCurrentSnapshot ( int vmHandle, out int snapshotHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixVM_GetNamedSnapshot ( int vmHandle, string name, out int snapshotHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RemoveSnapshot ( int vmHandle, int snapshotHandle, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RevertToSnapshot ( int vmHandle, int snapshotHandle, VixVMPowerOpOptions options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CreateSnapshot ( int vmHandle, string name, string description, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_EnableSharedFolders ( int vmHandle, bool enabled, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_GetNumSharedFolders ( int vmHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_GetSharedFolderState ( int vmHandle, int index, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_SetSharedFolderState ( int vmHandle, string shareName, string hostPathName, int flags, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_AddSharedFolder ( int vmHandle, string shareName, string hostPathName, int flags, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_RemoveSharedFolder ( int vmHandle, string shareName, int flags, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_CaptureScreenImage ( int vmHandle, int captureType, int additionalProperties, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_Clone ( int vmHandle, int snapshotHandle, VixCloneType cloneType, string destConfigPathName, int options, int propertyListHandle, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_UpgradeVirtualHardware ( int vmHandle, int options, VixEventProc? callbackProc, IntPtr clientData );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixVM_InstallTools ( int vmHandle, int options, string commandLineArgs, VixEventProc? callbackProc, IntPtr clientData );

    // Job functions
    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_Wait ( int jobHandle, int firstPropertyID /*, ...*/ );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_CheckCompletion ( int jobHandle, out bool complete );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_GetError ( int jobHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern int VixJob_GetNumProperties ( int jobHandle, int resultPropertyID );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_GetNthProperties ( int jobHandle, int index, int propertyID /*, ...*/ );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_GetNthProperties ( int jobHandle, int index, int propertyId, out IntPtr value, int terminator );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_GetNthProperties ( int jobHandle, int index, int propertyId, out int value, int terminator );

    // Snapshot tree functions
    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixSnapshot_GetNumChildren ( int parentSnapshotHandle, out int numChildSnapshots );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixSnapshot_GetChild ( int parentSnapshotHandle, int index, out int childSnapshotHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixSnapshot_GetParent ( int snapshotHandle, out int parentSnapshotHandle );

    [DllImport ( VixDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi )]
    public static extern ulong VixJob_Wait ( int jobHandle, int firstPropertyID, out int resultHandle, int terminator );

    private static int GetJobResultHandle ( int jobHandle )
    {
        int resultHandle = VixApi.VIX_INVALID_HANDLE;
        ulong err = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_JOB_RESULT_HANDLE, out resultHandle, VixApi.VIX_PROPERTY_NONE );
        if ( VixApi.VIX_OK == err )
        {
            return resultHandle;
        }
        return VixApi.VIX_INVALID_HANDLE;
    }
}
}
