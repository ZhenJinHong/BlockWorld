float3x3 RotateY(float angle)
{
    float c, s;
    sincos(angle, s, c);
    return float3x3(c, 0.0f, s,
                    0.0f, 1.0f, 0.0f,
                    -s, 0.0f, c);
}
 float3x3 EulerXYZ(float3 xyz)
{
    // return mul(rotateZ(xyz.z), mul(rotateY(xyz.y), rotateX(xyz.x)));
    float3 s, c;
    sincos(xyz,  s,  c);
    return float3x3(
        c.y * c.z,  c.z * s.x * s.y - c.x * s.z,    c.x * c.z * s.y + s.x * s.z,
        c.y * s.z,  c.x * c.z + s.x * s.y * s.z,    c.x * s.y * s.z - c.z * s.x,
        -s.y,       c.y * s.x,                      c.x * c.y
        );
}
float3x3 EulerYZX(float3 xyz)
{
    float3 s, c;
    sincos(xyz, s, c);
    return float3x3(
        c.y * c.z,                      -s.z,       c.z * s.y,
        s.x * s.y + c.x * c.y * s.z,    c.x * c.z,  c.x * s.y * s.z - c.y * s.x,
        c.y * s.x * s.z - c.x * s.y,    c.z * s.x,  c.x * c.y + s.x * s.y * s.z
        );
}
float3x3 EulerZXY(float3 xyz)
{
    float3 s, c;
    sincos(xyz, s, c);
    return float3x3(
        c.y * c.z + s.x * s.y * s.z,    c.z * s.x * s.y - c.y * s.z,    c.x * s.y,
        c.x * s.z,                      c.x * c.z,                      -s.x,
        c.y * s.x * s.z - c.z * s.y,    c.y * c.z * s.x + s.y * s.z,    c.x * c.y
        );
}
float3x3 EulerZYX(float3 xyz)
{
    float3 s, c;
    sincos(xyz, s, c);
    return float3x3(
        c.y * c.z,                    -c.y * s.z,                   s.y,
        c.z * s.x * s.y + c.x * s.z,  c.x * c.z - s.x * s.y * s.z,  -c.y * s.x,
        s.x * s.z - c.x * c.z * s.y,  c.z * s.x + c.x * s.y * s.z,  c.x * c.y
        );
}
float3x3 angleAxis3x3(float angle, float3 axis)
{
	float c, s;
	sincos(angle, s, c);

	float t = 1 - c;
	float x = axis.x;
	float y = axis.y;
	float z = axis.z;

	return float3x3
	(
		t * x * x + c, t * x * y - s * z, t * x * z + s * y,
		t * x * y + s * z, t * y * y + c, t * y * z - s * x,
		t * x * z - s * y, t * y * z + s * x, t * z * z + c
	);
}