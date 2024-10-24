namespace Sensify.Extensions;

public static class StringExtensions
{
    public static ReadOnlySpan<byte> ToHexBytes(this ReadOnlySpan<char> str){

        if(str.Length % 2 != 0 ) throw new ArgumentOutOfRangeException(nameof(str), str.ToString(), "Not a valid hex string.");
        var length = str.Length;
        Span<byte> results = new byte[length/2];

        for(var i = 0; i < length; i+=2){
            
            var char1 = str[i];
            var char2 = str[i+1];

            ref var value = ref results[i/2];

            value = char1 switch{
                >= '0' and <= '9' => (byte)(char1 - '0'),
                >= 'a' and <= 'f' => (byte)(char1 - 'a' + 10),
                >= 'A' and <= 'F' => (byte)(char1 - 'A' + 10),
                _ => throw new InvalidHexCharException(char1)
            };

            value = (byte)(value << 4);
            
            value |= char2 switch{
                >= '0' and <= '9' => (byte)(char2 - '0'),
                >= 'a' and <= 'f' => (byte)(char2 - 'a' + 10),
                >= 'A' and <= 'F' => (byte)(char2 - 'A' + 10),
                _ => throw new InvalidHexCharException(char2)
            };

        }

        return results;
    }

    public static ReadOnlySpan<byte> ToHexBytes(this string str) => str.AsSpan().ToHexBytes();

    public class InvalidHexCharException : Exception
    {
        public char InvalidChar {get;}

        public InvalidHexCharException(char ch){
            InvalidChar = ch;
        }
    }
}