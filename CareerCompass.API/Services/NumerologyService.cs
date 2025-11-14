using System.Text;
using CareerCompass.Api.Dtos;

namespace CareerCompass.Api.Services
{
    public class NumerologyService : INumerologyService
    {
        public CareerResponseDto GetCareerRecommendation(string fullName, DateTime birthDate)
        {
            var numbers = CalculateNumbers(fullName, birthDate);

            // Weighted score based on the plan:
            // Life Path 35%, Expression 25%, Personality 20%, Soul Urge 10%, Destiny 10%
            double weightedScore =
                numbers.LifePath * 0.35 +
                numbers.Expression * 0.25 +
                numbers.Personality * 0.20 +
                numbers.SoulUrge * 0.10 +
                numbers.Destiny * 0.10;

            int primaryNumber = ReduceToSingleDigit((int)Math.Round(weightedScore));

            var (career, category, description) = MapNumberToCareer(primaryNumber, numbers);

            return new CareerResponseDto
            {
                Career = career,
                Category = category,
                Description = description,
                Numbers = new NumerologyNumbersDto
                {
                    LifePath = numbers.LifePath,
                    Expression = numbers.Expression,
                    Personality = numbers.Personality,
                    SoulUrge = numbers.SoulUrge,
                    Destiny = numbers.Destiny
                }
            };
        }

        // ----- Core numerology calculations -----

        private (int LifePath, int Expression, int Personality, int SoulUrge, int Destiny)
            CalculateNumbers(string fullName, DateTime birthDate)
        {
            int lifePath = CalculateLifePathNumber(birthDate);

            string cleanedName = CleanName(fullName);
            string vowels = new string(cleanedName.Where(IsVowel).ToArray());
            string consonants = new string(cleanedName.Where(c => !IsVowel(c)).ToArray());

            int expression = ReduceToSingleDigit(GetNameNumber(cleanedName));
            int soulUrge = ReduceToSingleDigit(GetNameNumber(vowels));
            int personality = ReduceToSingleDigit(GetNameNumber(consonants));
            int destiny = ReduceToSingleDigit(expression + lifePath); // simple combo

            return (lifePath, expression, personality, soulUrge, destiny);
        }

        private int CalculateLifePathNumber(DateTime birthDate)
        {
            // Standard: sum all digits of yyyyMMdd then reduce
            string digits = birthDate.ToString("yyyyMMdd");
            int sum = digits.Sum(c => c - '0');
            return ReduceToSingleDigit(sum);
        }

        private int ReduceToSingleDigit(int number)
        {
            // Reduce until 1–9 (ignoring master numbers for simplicity)
            number = Math.Abs(number);

            while (number > 9)
            {
                int sum = 0;
                while (number > 0)
                {
                    sum += number % 10;
                    number /= 10;
                }
                number = sum;
            }

            return number == 0 ? 1 : number;
        }

        private string CleanName(string name)
        {
            var sb = new StringBuilder();
            foreach (char c in name.ToUpperInvariant())
            {
                if (char.IsLetter(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private bool IsVowel(char c)
        {
            return "AEIOU".Contains(c);
        }

        private int GetNameNumber(string name)
        {
            int sum = 0;
            foreach (char c in name)
            {
                sum += GetLetterValue(c);
            }
            return sum;
        }

        // Pythagorean numerology mapping A=1 ... I=9, then J=1 again, etc.
        private int GetLetterValue(char c)
        {
            if (!char.IsLetter(c))
                return 0;

            int position = (c - 'A') + 1; // A = 1, B = 2, ...
            int value = position % 9;
            return value == 0 ? 9 : value;
        }

        private (string Career, string Category, string Description)
            MapNumberToCareer(int primaryNumber,
                (int LifePath, int Expression, int Personality, int SoulUrge, int Destiny) numbers)
        {
            switch (primaryNumber)
            {
                case 1:
                    return ("IT Project Manager", "Leadership & Management",
                        "You show strong initiative and ownership. You’re suited for roles that let you lead teams, make decisions, and drive outcomes.");
                case 2:
                    return ("Business Analyst", "Collaboration & Support",
                        "You’re naturally diplomatic and good at listening, making you ideal for roles that sit between business and technical teams.");
                case 3:
                    return ("Digital Marketer / Content Creator", "Creative & Communication",
                        "You communicate well and think creatively, fitting roles where you craft messages, campaigns, or content.");
                case 4:
                    return ("Systems Administrator", "Operations & Stability",
                        "You’re detail-oriented and structured, matching roles focused on stability, reliability, and well-defined processes.");
                case 5:
                    return ("IT Consultant", "Change & Adaptability",
                        "You adapt quickly and enjoy variety, fitting consulting, client-facing, or fast-paced environments.");
                case 6:
                    return ("Trainer / Instructor", "Teaching & Support",
                        "You have a caring and supportive energy, fitting mentoring, training, or help-desk related roles.");
                case 7:
                    return ("Data Analyst / Researcher", "Research & Analysis",
                        "You’re analytical and introspective, aligning with roles involving data, research, or deep problem solving.");
                case 8:
                    return ("IT Manager / Architect", "Business & Strategy",
                        "You’re ambitious and practical, ideal for strategic roles that balance tech with budgets and results.");
                case 9:
                default:
                    return ("Nonprofit Tech Specialist", "Impact & Service",
                        "You’re driven by meaning and impact, fitting roles where technology supports communities, education, or social good.");
            }
        }
    }
}
