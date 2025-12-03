using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient http = new HttpClient();

    static async Task<(double lat, double lon)?> GetIssPositionAsync()
    {
        try
        {
            string json = await http.GetStringAsync("http://api.open-notify.org/iss-now.json");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("message", out var msgProp) ||
                !string.Equals(msgProp.GetString(), "success", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var pos = root.GetProperty("iss_position");
            string latStr = pos.GetProperty("latitude").GetString();
            string lonStr = pos.GetProperty("longitude").GetString();

            double lat = double.Parse(latStr, CultureInfo.InvariantCulture);
            double lon = double.Parse(lonStr, CultureInfo.InvariantCulture);

            // Open Notify returns lon in -180..180 already, so we’re good
            return (lat, lon);
        }
        catch
        {
            return null;
        }
    }

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        string[] worldMap = {
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣄⣠⣀⡀⣀⣠⣤⣤⣤⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣄⢠⣠⣼⣿⣿⣿⣟⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⢠⣤⣦⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠰⢦⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⣼⣿⣟⣾⣿⣽⣿⣿⣅⠈⠉⠻⣿⣿⣿⣿⣿⡿⠇⠀⠀⠀⠀⠀⠉⠀⠀⠀⠀⠀⢀⡶⠒⢉⡀⢠⣤⣶⣶⣿⣷⣆⣀⡀⠀⢲⣖⠒⠀⠀⠀⠀⠀⠀⠀",
"⢀⣤⣾⣶⣦⣤⣤⣶⣿⣿⣿⣿⣿⣿⣽⡿⠻⣷⣀⠀⢻⣿⣿⣿⡿⠟⠀⠀⠀⠀⠀⠀⣤⣶⣶⣤⣀⣀⣬⣷⣦⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣦⣤⣦⣼⣀⠀",
"⠈⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠛⠓⣿⣿⠟⠁⠘⣿⡟⠁⠀⠘⠛⠁⠀⠀⢠⣾⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠏⠙⠁",
"⠀⠸⠟⠋⠀⠈⠙⣿⣿⣿⣿⣿⣿⣷⣦⡄⣿⣿⣿⣆⠀⠀⠀⠀⠀⠀⠀⠀⣼⣆⢘⣿⣯⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡉⠉⢱⡿⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⡿⠦⠀⠀⠀⠀⠀⠀⠀⠙⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⡗⠀⠈⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⣿⣿⣿⣿⣿⣿⣿⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⣿⣉⣿⡿⢿⢷⣾⣾⣿⣞⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠋⣠⠟⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⣿⣿⠿⠿⣿⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣾⣿⣿⣷⣦⣶⣦⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣷⠈⠛⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠻⣿⣤⡖⠛⠶⠤⡀⠀⠀⠀⠀⠀⠀⠀⢰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠁⠙⣿⣿⠿⢻⣿⣿⡿⠋⢩⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⠧⣤⣦⣤⣄⡀⠀⠀⠀⠀⠀⠘⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇⠀⠀⠀⠘⣧⠀⠈⣹⡻⠇⢀⣿⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⣿⣿⣿⣿⣤⣀⡀⠀⠀⠀⠀⠀⠀⠈⢽⣿⣿⣿⣿⣿⠋⠀⠀⠀⠀⠀⠀⠀⠀⠹⣷⣴⣿⣷⢲⣦⣤⡀⢀⡀⠀⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢿⣿⣿⣿⣿⣿⣿⠟⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⣿⣿⣷⢀⡄⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠂⠛⣆⣤⡜⣟⠋⠙⠂⠀⠀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⣿⣿⣿⣿⠟⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⣿⣿⣿⠉⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣤⣾⣿⣿⣿⣿⣆⠀⠰⠄⠀⠉⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⣿⡿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⣿⡿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⣿⠿⠿⣿⣿⣿⠇⠀⠀⢀⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⡿⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⡇⠀⠀⢀⣼⠗⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⠃⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠁⠀⠀⠀",
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠒⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀"
        };

        int mapHeight = worldMap.Length;
        int mapWidth = worldMap[0].Length;

        // Latitude range represented by the map (approx mid-Earth: 60°N to 60°S)
        const double LatMax = 60.0;
        const double LatMin = -60.0;

        // Left margin labels per row (lat lines)
        string[] latLabels = Enumerable.Repeat("        ", mapHeight).ToArray();

        int LatToRow(double lat)
        {
            double clamped = Math.Max(LatMin, Math.Min(LatMax, lat));
            double t = (LatMax - clamped) / (LatMax - LatMin);    // 0 at LatMax, 1 at LatMin
            return (int)Math.Round(t * (mapHeight - 1));          // 0..mapHeight-1
        }

        int LonToCol(double lon)
        {
            double clamped = Math.Max(-180.0, Math.Min(180.0, lon));
            double t = (clamped + 180.0) / 360.0;                 // -180 → 0, +180 → 1
            return (int)Math.Round(t * (mapWidth - 1));           // 0..mapWidth-1
        }

        double RowToLat(int row)
        {
            double t = (double)row / (mapHeight - 1);
            return LatMax - t * (LatMax - LatMin);
        }

        double ColToLon(int col)
        {
            double t = (double)col / (mapWidth - 1);
            return -180.0 + t * 360.0;
        }

        void SetLatLabel(double latDeg, string label)
        {
            int row = LatToRow(latDeg);
            if (row >= 0 && row < mapHeight)
                latLabels[row] = label;
        }

        // Degree lines on the left, aligned with mapping
        SetLatLabel(60, " 60°N | ");
        SetLatLabel(30, " 30°N | ");
        SetLatLabel(0, "  0°  | ");
        SetLatLabel(-30, " 30°S | ");
        SetLatLabel(-60, " 60°S | ");

        // === CONTINENT REGIONS (bounding boxes in real lat/lon) ===
        var regionDefs = new (string Code, double MinLat, double MaxLat, double MinLon, double MaxLon)[]
        {
            ("NA",  20, 55,  -140,  -50), // North America
            ("SA", -60, 15,   -90,  -30), // South America
            ("EU",  30, 60,   -10,   40), // Europe
            ("AF", -10, 20,   -20,   50), // Africa
            ("AS",   5, 80,    45,  180), // Asia
            ("AU", -50, -10,  110,  160), // Australia
        };

        // Accumulators to find visual centers of land inside each region
        double[] rowSum = new double[regionDefs.Length];
        double[] colSum = new double[regionDefs.Length];
        int[] count = new int[regionDefs.Length];

        bool IsLand(char ch) =>
            "⣿⣟⣯⣷⣄⣆⣾⣶⣤⣠⡀⡄⡆".Contains(ch);

        // Scan map once
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                char ch = worldMap[i][j];
                if (!IsLand(ch)) continue;

                double latHere = RowToLat(i);
                double lonHere = ColToLon(j);

                for (int r = 0; r < regionDefs.Length; r++)
                {
                    var reg = regionDefs[r];
                    if (latHere >= reg.MinLat && latHere <= reg.MaxLat &&
                        lonHere >= reg.MinLon && lonHere <= reg.MaxLon)
                    {
                        rowSum[r] += i;
                        colSum[r] += j;
                        count[r]++;
                    }
                }
            }
        }

        // Build markers (then force rows for NA, EU, AF, AU)
        var markers = new (int Row, int Col, string Text)[regionDefs.Length];
        for (int r = 0; r < regionDefs.Length; r++)
        {
            int centerRow;
            int centerCol;

            if (count[r] > 0)
            {
                centerRow = (int)Math.Round(rowSum[r] / count[r]);
                centerCol = (int)Math.Round(colSum[r] / count[r]);
            }
            else
            {
                double midLat = (regionDefs[r].MinLat + regionDefs[r].MaxLat) / 2.0;
                double midLon = (regionDefs[r].MinLon + regionDefs[r].MaxLon) / 2.0;
                centerRow = LatToRow(midLat);
                centerCol = LonToCol(midLon);
            }

            if (regionDefs[r].Code == "NA" || regionDefs[r].Code == "EU")
            {
                centerRow = LatToRow(25.0);   // 25°N
            }
            else if (regionDefs[r].Code == "AF")
            {
                centerRow = LatToRow(-10.0);  // 10°S
            }
            else if (regionDefs[r].Code == "AU")
            {
                centerRow = Math.Min(mapHeight - 1, centerRow + 1); // one row lower
            }

            markers[r] = (centerRow, centerCol, regionDefs[r].Code);
        }

        bool loading = true;
        (double lat, double lon) lastPos = (0, 0);
        bool hasLastPos = false;

        while (true)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            // First time: show loading splash once
            if (loading)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\n                ACQUIRING SATELLITE SIGNAL...");
                Console.WriteLine("                Contacting ISS tracking service...");
                Console.ResetColor();
                for (int i = 0; i < worldMap.Length; i++)
                    Console.WriteLine(worldMap[i]);
                Console.WriteLine("\n                Please wait — establishing lock...");
                await Task.Delay(3000);
                loading = false;
                continue;
            }

            // Get real ISS position
            var pos = await GetIssPositionAsync();
            double lat, lon;

            if (pos.HasValue)
            {
                lat = pos.Value.lat;
                lon = pos.Value.lon;
                lastPos = pos.Value;
                hasLastPos = true;
            }
            else if (hasLastPos)
            {
                lat = lastPos.lat;
                lon = lastPos.lon;
            }
            else
            {
                // No data at all yet; default to (0,0)
                lat = 0;
                lon = 0;
            }

            // MAIN DISPLAY
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("    INTERNATIONAL SPACE STATION — LIVE ORBITAL TRACKING");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"     LAT: {lat,6:F2}°   LON: {lon,7:F2}°   ALT: ~408 km   SPEED: ~27,600 km/h\n");
            Console.ResetColor();

            // Shift ISS down 2 rows and left 1 column
            int y = LatToRow(lat) + 4;
            int x = LonToCol(lon) - 2;

            // Clamp to protect against IndexOutOfRange
            y = Math.Max(1, Math.Min(mapHeight - 2, y));
            x = Math.Max(1, Math.Min(mapWidth - 4, x));  // 4 = space needed for "ISS"


            // DRAW MAP
            for (int i = 0; i < mapHeight; i++)
            {
                char[] c = worldMap[i].ToCharArray();
                int[] markerOwner = Enumerable.Repeat(-1, mapWidth).ToArray();
                bool[] issHere = new bool[mapWidth];

                // Continent labels (red, centered)
                for (int r = 0; r < markers.Length; r++)
                {
                    var m = markers[r];
                    if (m.Row != i) continue;

                    int startCol = m.Col - m.Text.Length / 2;
                    for (int k = 0; k < m.Text.Length; k++)
                    {
                        int idx = startCol + k;
                        if (idx >= 0 && idx < c.Length)
                        {
                            c[idx] = m.Text[k];
                            markerOwner[idx] = r;
                        }
                    }
                }

                // ISS symbol (yellow, overrides)
                bool rowHasISS = (i >= y - 1 && i <= y + 1);
                if (rowHasISS)
                {
                    int drawX = x;

                    if (i == y - 1)
                    {
                        if (drawX < c.Length) { c[drawX] = '✦'; issHere[drawX] = true; }
                        if (drawX + 1 < c.Length) { c[drawX + 1] = '✦'; issHere[drawX + 1] = true; }
                    }
                    if (i == y)
                    {
                        if (drawX < c.Length) { c[drawX] = 'I'; issHere[drawX] = true; }
                        if (drawX + 1 < c.Length) { c[drawX + 1] = 'S'; issHere[drawX + 1] = true; }
                        if (drawX + 2 < c.Length) { c[drawX + 2] = 'S'; issHere[drawX + 2] = true; }
                    }
                    if (i == y + 1)
                    {
                        if (drawX < c.Length) { c[drawX] = '✦'; issHere[drawX] = true; }
                        if (drawX + 1 < c.Length) { c[drawX + 1] = '✦'; issHere[drawX + 1] = true; }
                    }
                }

                // Left-side latitude label
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(latLabels[i]);

                // Per-character coloring
                for (int j = 0; j < c.Length; j++)
                {
                    char ch = c[j];

                    if (issHere[j])
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // ISS
                    }
                    else if (markerOwner[j] >= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;    // continent labels
                    }
                    else if ("⣿⣟⣯⣷⣄⣆⣾⣶⣤⣠⡀⡄⡆".Contains(ch))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;  // land
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue; // water
                    }

                    Console.Write(ch);
                }

                Console.WriteLine();
            }

            // LONGITUDE SCALE
            char[] tickLine = Enumerable.Repeat(' ', mapWidth).ToArray();
            char[] labelLine = Enumerable.Repeat(' ', mapWidth).ToArray();

            void MarkTick(int lonDeg)
            {
                int col = LonToCol(lonDeg);
                if (col >= 0 && col < mapWidth)
                    tickLine[col] = '│';
            }

            void PlaceLabel(int lonDeg, string text)
            {
                int col = LonToCol(lonDeg);
                int start = col - text.Length / 2;
                for (int j = 0; j < text.Length; j++)
                {
                    int idx = start + j;
                    if (idx >= 0 && idx < mapWidth)
                        labelLine[idx] = text[j];
                }
            }

            MarkTick(-180);
            MarkTick(-90);
            MarkTick(0);
            MarkTick(90);
            MarkTick(180);

            PlaceLabel(-180, "-180°");
            PlaceLabel(-90, "-90°");
            PlaceLabel(0, "  0° ");
            PlaceLabel(90, " 90°");
            PlaceLabel(180, "180°");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n        ");
            Console.WriteLine(new string(tickLine));
            Console.Write("        ");
            Console.WriteLine(new string(labelLine));
            Console.ResetColor();

            Console.WriteLine(
                "\n           ISS = BIG GLOWING YELLOW ✦ / ISS • Updates every 2s • Press any key to exit");

            if (Console.KeyAvailable)
                break;

            await Task.Delay(2000);
        }

        Console.CursorVisible = true;
        Console.ResetColor();
        Console.WriteLine("\nYOUR MASTERPIECE IS COMPLETE — This is going to be legendary on GitHub.");
    }
}
