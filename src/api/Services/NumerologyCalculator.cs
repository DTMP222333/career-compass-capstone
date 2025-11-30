using System;
using System.Linq;

namespace Api.Services
{
    public static class NumerologyCalculator
    {
        // Reduce to a single digit BUT preserve master numbers 11, 22, 33
        public static int Reduce(int number)
        {
            while (number > 9)
            {
                number = number
                    .ToString()
                    .Sum(c => c - '0');
            }

            return number;


            while (number > 9)
            {
                number = number
                    .ToString()
                    .Sum(c => c - '0');
            }

            return number;
        }

        // Convert name → total numerology value (A=1 … Z=26)
        public static int NameToNumber(string name)
        {
            return name
                .ToUpper()
                .Where(char.IsLetter)
                .Sum(c => (c - 'A' + 1));
        }

        // Life Path = sum of DOB digits (YYYY + MM + DD)
        public static int GetLifePath(DateTime dob)
        {
            int yearSum = dob.Year.ToString().Sum(c => c - '0');
            int monthSum = dob.Month.ToString().Sum(c => c - '0');
            int daySum = dob.Day.ToString().Sum(c => c - '0');

            return Reduce(yearSum + monthSum + daySum);
        }

        // Expression number (full name)
        public static int GetExpression(string fullName)
        {
            return Reduce(NameToNumber(fullName));
        }

        // Soul Urge (vowels only)
        public static int GetSoulUrge(string fullName)
        {
            string vowels = "AEIOU";
            int sum = fullName
                .ToUpper()
                .Where(c => vowels.Contains(c))
                .Sum(c => (c - 'A' + 1));

            return Reduce(sum);
        }

        // Personality (consonants only)
        public static int GetPersonality(string fullName)
        {
            string vowels = "AEIOU";
            int sum = fullName
                .ToUpper()
                .Where(c => char.IsLetter(c) && !vowels.Contains(c))
                .Sum(c => (c - 'A' + 1));

            return Reduce(sum);
        }

        // Birthday = reduce(dob.Day)
        public static int GetBirthDay(DateTime dob)
        {
            return Reduce(dob.Day);
        }
        // Career description based on Life Path
        public static string GetCareerDescription(int lifePath)
        {
            return lifePath switch
            {
                1 => "You are a natural leader. Roles in management, entrepreneurship, and decision-making suit you well.",
                2 => "You thrive in cooperative environments. Ideal careers include HR, counseling, teaching, and mediation.",
                3 => "You are creative and expressive. You excel in design, writing, communication, and entertainment fields.",
                4 => "You are disciplined and practical. Engineering, IT, operations, and project management fit you well.",
                5 => "You are adaptable and energetic. Great for sales, marketing, travel, hospitality, and public-facing roles.",
                6 => "You are nurturing and responsible. Ideal paths include healthcare, education, coaching, and support roles.",
                7 => "You are analytical and introspective. Strong in research, data, IT, and scientific fields.",
                8 => "You are business-focused and ambitious. Great in finance, management, real estate, or executive roles.",
                9 => "You are compassionate and artistic. You thrive in creative, nonprofit, humanitarian, or counseling roles.",
                _ => "No career description found for this life path number."
            };
        }

        // Short career title based on Life Path
        public static string GetCareerTitle(int lifePath)
        {
            return lifePath switch
            {
                1 => "Leadership, Management, Entrepreneurship",
                2 => "HR, Counseling, Teaching, Mediation",
                3 => "Design, Writing, Communication, Arts",
                4 => "Engineering, IT, Operations, Project Management",
                5 => "Sales, Marketing, Travel, Hospitality",
                6 => "Healthcare, Education, Coaching",
                7 => "Research, Data Science, IT, Analysis",
                8 => "Finance, Business, Real Estate, Executive Roles",
                9 => "Creative, Nonprofit, Humanitarian Work",
                _ => "General Career Fields"
            };
        }

    }


}
