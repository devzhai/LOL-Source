void DrawManager::DrawLine(const XMFLOAT2& point1, const XMFLOAT2& point2) const
{
    constexpr int pointsCount{ 2 };
 
    XMFLOAT2 points[pointsCount]
    {
        {point1}, {point2}
    };
 
    TransformCoords(points, pointsCount);
 
    const auto color{ D3DXCOLOR(0.0f, 1.0f, 1.0f, 1.0f) };
    const VERTEX vertices[pointsCount]
    {
        {points[0].x, points[0].y, 0.0f, color},
        {points[1].x, points[1].y, 0.0f, color}
    };
 
    // copy the vertices into the buffer
    D3D11_MAPPED_SUBRESOURCE ms;
    m_pDevCon->Map(m_pVBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &ms);
    memcpy(ms.pData, vertices, sizeof(vertices));
    m_pDevCon->Unmap(m_pVBuffer, NULL);
 
    m_pDevCon->IASetPrimitiveTopology(D3D10_PRIMITIVE_TOPOLOGY_LINELIST);
 
    // draw the vertex buffer to the back buffer
    m_pDevCon->Draw(pointsCount, 0);
}
 
Vector2 getCordByAngle(float angle, float distance)
{
    angle = D3DX_PI * angle / 180;
    Vector2 point{};
    point.X = distance * cos(angle);
    point.Y = distance * sin(angle);
 
    return point;
}
 
const auto lPlayerPos{ localPlayer->GetPos() };
            const auto aaRange{ localPlayer->GetAttackRange() };
            Vector2 Prev;
            for (int i = 0; i <= 360; i++)
            {
                const float angle = (i / 360.0f) * 360.f;
                Vector3 LocAlpha = lPlayerPos;
                const Vector2 m = getCordByAngle(angle, aaRange);
                LocAlpha.X += m.X;
                LocAlpha.Z += m.Y;
                const Vector2 ScreenLoc = Vector2::WorldToScreen(LocAlpha);
                if (i == 0)
                    Prev = ScreenLoc;
 
                g::DRAW_MANAGER.DrawLine({Prev.X, Prev.Y}, {ScreenLoc.X, ScreenLoc.Y});
                Prev = ScreenLoc;
            }