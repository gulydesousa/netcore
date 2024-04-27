
string stype = "Type";
string sbytes = "Byte(s) of Memory";
string smin = "Min";
string smax = "Max";

WriteLine($"{stype,-10} {sbytes,-20} {smin,32} {smax,45}");

WriteLine($"{"byte",-10} {sizeof(byte),-20} {byte.MinValue,32} {byte.MaxValue,45}");
WriteLine($"{"sbyte",-10} {sizeof(sbyte),-20} {sbyte.MinValue,32} {sbyte.MaxValue,45}");
WriteLine($"{"short",-10} {sizeof(short),-20} {short.MinValue,32} {short.MaxValue,45}");
WriteLine($"{"ushort",-10} {sizeof(ushort),-20} {ushort.MinValue,32} {ushort.MaxValue,45}");
WriteLine($"{"int",-10} {sizeof(int),-20} {int.MinValue,32} {int.MaxValue,45}");
WriteLine($"{"uint",-10} {sizeof(uint),-20} {uint.MinValue,32} {uint.MaxValue,45}");
WriteLine($"{"long",-10} {sizeof(long),-20} {long.MinValue,32} {long.MaxValue,45}");
WriteLine($"{"ulong",-10} {sizeof(ulong),-20} {ulong.MinValue,32} {ulong.MaxValue,45}");
// WriteLine($"{"Int128",-10} {sizeof(Int128),-4} {Int128.MinValue,32} {Int128.MaxValue,45}");
WriteLine($"{"float",-10} {sizeof(float),-20} {float.MinValue,32} {float.MaxValue,45}");
WriteLine($"{"double",-10} {sizeof(double),-20} {double.MinValue,32} {double.MaxValue,45}");
WriteLine($"{"decimal",-10} {sizeof(decimal),-20} {decimal.MinValue,32} {decimal.MaxValue,45}");
WriteLine($"{"char",-10} {sizeof(char),-20} {char.MinValue,32} {char.MaxValue,45}");
WriteLine($"{"bool",-10} {sizeof(bool),-20} {bool.FalseString,32} {bool.TrueString,45}");

//Escribir un millon usando la notación con _ para mejorar la legibilidad
WriteLine(1_000_000);

//Escribir 2.3 como float y como double

WriteLine(2.3f); //FLOAT
WriteLine(2.3d); //DOUBLE

//Escribir el 2.3 como decimal
WriteLine(2.3m); //DECIMAL

