namespace AdventOfCode2022.Day.Six;

public class Subroutine
{
    public string DataStreamBuffer { get; set; }

    public int GetStartMarker(int distinctCharactersRequired)
    {
        var buffer = new Queue<char>();
        for (int i = 0; i < DataStreamBuffer.Length; i++)
        {
            buffer.Enqueue(DataStreamBuffer[i]);
            if (buffer.Count > distinctCharactersRequired)
            {
                buffer.Dequeue();
            }
            
            if (buffer.Count == distinctCharactersRequired)
            {
                if (buffer.Distinct().Count() == distinctCharactersRequired) return ++i;
            }
        }

        return -1;
    }

    
}