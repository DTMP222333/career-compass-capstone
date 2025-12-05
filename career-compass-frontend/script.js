// === API BASE URL ===
const apiBaseUrl = "http://74.235.81.165:5000";


// Detect page
const currentPage = window.location.pathname.split("/").pop();

//////////////////////////////////////////////////////////////////
// LOGIN PAGE
//////////////////////////////////////////////////////////////////
if (currentPage === "login.html") {
    document.addEventListener("DOMContentLoaded", () => {

        const loginForm = document.getElementById("loginForm");
        const loginError = document.getElementById("loginError");

        // Custom dropdowns
        const ddYear = document.getElementById("yearDropdown");
        const ddMonth = document.getElementById("monthDropdown");
        const ddDay = document.getElementById("dayDropdown");

        const allDropdowns = [ddYear, ddMonth, ddDay];

        // Selected values
        let selectedYear = "";
        let selectedMonth = "";
        let selectedDay = "";

        function closeAll() {
            allDropdowns.forEach(d => d.classList.remove("open"));
        }

        // YEAR
        const yearList = ddYear.querySelector(".dropdown-list");
        const currentYear = new Date().getFullYear();
        for (let y = currentYear; y >= 1970; y--) {
            const item = document.createElement("div");
            item.className = "dropdown-item";
            item.textContent = y;
            item.dataset.value = y;
            yearList.appendChild(item);
        }

        // MONTH
        const monthList = ddMonth.querySelector(".dropdown-list");
        const months = [
            { name: "January", value: "01" }, { name: "February", value: "02" },
            { name: "March", value: "03" }, { name: "April", value: "04" },
            { name: "May", value: "05" }, { name: "June", value: "06" },
            { name: "July", value: "07" }, { name: "August", value: "08" },
            { name: "September", value: "09" }, { name: "October", value: "10" },
            { name: "November", value: "11" }, { name: "December", value: "12" }
        ];
        months.forEach(m => {
            const item = document.createElement("div");
            item.className = "dropdown-item";
            item.textContent = m.name;
            item.dataset.value = m.value;
            monthList.appendChild(item);
        });

        // DAY
        const dayList = ddDay.querySelector(".dropdown-list");
        for (let d = 1; d <= 31; d++) {
            const item = document.createElement("div");
            item.className = "dropdown-item";
            item.textContent = d.toString().padStart(2, "0");
            item.dataset.value = d.toString().padStart(2, "0");
            dayList.appendChild(item);
        }

        // SETUP DROPDOWN FUNCTION
        function setupDropdown(dropdown, type) {
            const display = dropdown.querySelector(".dropdown-selected");
            const list = dropdown.querySelector(".dropdown-list");

           dropdown.addEventListener("click", function (e) {
    e.stopPropagation();

    // Only open if clicking the display area, not list items
    if (!e.target.classList.contains("dropdown-item")) {
        closeAll();
        dropdown.classList.add("open");
    }
});


            list.querySelectorAll(".dropdown-item").forEach(item => {
                item.addEventListener("click", function () {
                    display.textContent = item.textContent;
                    dropdown.classList.remove("open");

                    if (type === "year") selectedYear = item.dataset.value;
                    if (type === "month") selectedMonth = item.dataset.value;
                    if (type === "day") selectedDay = item.dataset.value;
                });
            });
        }

        setupDropdown(ddYear, "year");
        setupDropdown(ddMonth, "month");
        setupDropdown(ddDay, "day");

        document.addEventListener("click", closeAll);

      // SUBMIT LOGIN FORM
loginForm.addEventListener("submit", (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();

    // ⭐ NEW: Name must contain letters only
    const nameRegex = /^[A-Za-z\s]+$/;

    if (!nameRegex.test(username)) {
        loginError.classList.remove("hidden");
        loginError.textContent = "Name can only contain letters.";
        return;
    }

    if (!username || !selectedYear || !selectedMonth || !selectedDay) {
        loginError.classList.remove("hidden");
        loginError.textContent = "Please fill all fields.";
        return;
    }

    const dob = `${selectedYear}-${selectedMonth}-${selectedDay}`;

    localStorage.setItem(
        "userData",
        JSON.stringify({
            fullName: username,
            dateOfBirth: dob
        })
    );

    window.location.href = "assessment.html";
});
    });
}

//////////////////////////////////////////////////////////////////
// ASSESSMENT PAGE
//////////////////////////////////////////////////////////////////
if (currentPage === "assessment.html") {

    document.addEventListener("DOMContentLoaded", () => {

        const loadingSection = document.getElementById("loading-section");
        const resultSection = document.getElementById("result-section");
        const errorSection = document.getElementById("error-section");

        const careerName = document.getElementById("careerName");
        const careerDescription = document.getElementById("careerDescription");

        // Load user info from Login page
        const user = JSON.parse(localStorage.getItem("userData"));
        if (!user) {
            alert("Please enter details again.");
            window.location.href = "login.html";
            return;
        }

        const requestBody = {
            fullName: user.fullName,
            dateOfBirth: user.dateOfBirth
        };

        // Call BOTH APIs at the same time
        Promise.all([
            fetch(`${apiBaseUrl}/api/Numerology/calculate`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(requestBody)
            }),
            fetch(`${apiBaseUrl}/api/Career/recommendation`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(requestBody)
            })
        ])
        .then(async ([numRes, careerRes]) => {

            const numerology = await numRes.json();
            const career = await careerRes.json();

            // Always use career API's title
            const careerTitle = career.career;

            // Fallback mini-descriptions (if API returns nothing)
            const shortFallback = {
                1: "You are a natural leader—great for management or entrepreneurial roles.",
                2: "You thrive in cooperative environments like HR, counseling, and teaching.",
                3: "Creative and expressive—strong in design, writing, and communication.",
                4: "Practical and disciplined—great for engineering, IT, and operations.",
                5: "Adaptable and energetic—ideal for sales, marketing, travel, hospitality.",
                6: "Nurturing and responsible—strong fit for healthcare, coaching, education.",
                7: "Analytical and introspective—great for research, data, and IT.",
                8: "Ambitious and strategic—excellent for finance, business, leadership.",
                9: "Compassionate and creative—aligned with nonprofit, art, humanitarian work."
            };

            // Use API → fallback to numerology API → fallback to short description
            const careerMeaning =
                career.description ||
                numerology.careerMeaning ||
                shortFallback[numerology.lifePath] ||
                "Career description unavailable.";

            // Save combined result for Learn More page
            const fullResult = {
                ...numerology,
                career: careerTitle,
                description: careerMeaning
            };

            localStorage.setItem("careerResult", JSON.stringify(fullResult));

            // Update UI
            careerName.textContent = careerTitle;
            careerDescription.textContent = careerMeaning;

            loadingSection.classList.add("hidden");
            resultSection.classList.remove("hidden");
        })
        .catch(err => {
            console.error("API ERROR:", err);
            loadingSection.classList.add("hidden");
            errorSection.classList.remove("hidden");
        });

        // Buttons
        document.getElementById("tryAgainBtn").onclick = () => window.location.href = "login.html";
        document.getElementById("learnMoreBtn").onclick = () => window.location.href = "learnmore.html";
    });
}



//////////////////////////////////////////////////////////////////
// LEARN MORE PAGE  (Uses backend full description)
//////////////////////////////////////////////////////////////////
if (currentPage === "learnmore.html") {

    document.addEventListener("DOMContentLoaded", () => {

        const result = JSON.parse(localStorage.getItem("careerResult"));

        if (!result) {
            alert("No data found. Please do assessment again.");
            window.location.href = "login.html";
            return;
        }

        // Fill number values
        document.getElementById("lifePathNum").textContent = result.lifePath;
        document.getElementById("expressionNum").textContent = result.expression;
        document.getElementById("soulUrgeNum").textContent = result.soulUrge;
        document.getElementById("personalityNum").textContent = result.personality;
        document.getElementById("birthDayNum").textContent = result.birthDay;

        // Set career name + full description from backend
        document.getElementById("careerName").textContent = result.career;
        document.getElementById("careerDescription").textContent = result.description;
    });
}
const pdfBtn = document.getElementById("downloadPdfBtn");

if (pdfBtn) {
    pdfBtn.addEventListener("click", async () => {
        const { jsPDF } = window.jspdf;

        const pdf = new jsPDF("p", "pt", "a4");
        const target = document.querySelector(".login-box");

        const canvas = await html2canvas(target, { scale: 2 });
        const imgData = canvas.toDataURL("image/png");

        const imgWidth = 550;
        const pageHeight = 780;
        const imgHeight = (canvas.height * imgWidth) / canvas.width;

        let heightLeft = imgHeight;
        let position = 20;

        pdf.addImage(imgData, "PNG", 30, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;

        while (heightLeft > 0) {
            pdf.addPage();
            position = heightLeft - imgHeight;
            pdf.addImage(imgData, "PNG", 30, position, imgWidth, imgHeight);
            heightLeft -= pageHeight;
        }

        pdf.save("Career-Compass-Report.pdf");
    });
}

// === EXPAND / COLLAPSE MAIN BOX ===
const expandToggle = document.querySelector('.center-toggle');
const expandBox = document.querySelector('.expand-box');

let expanded = false;

if (expandToggle) {
    expandToggle.addEventListener('click', () => {
        expanded = !expanded;

        if (expanded) {
            expandBox.style.maxHeight = expandBox.scrollHeight + "px";
            expandToggle.classList.add('active');
        } else {
            expandBox.style.maxHeight = "0px";
            expandToggle.classList.remove('active');
        }
    });
}


// === ACCORDION SECTIONS ===
const headers = document.querySelectorAll(".accordion-header");

if (headers.length > 0) {
    headers.forEach(header => {
        header.addEventListener("click", () => {
            header.classList.toggle("active");
            const content = header.nextElementSibling;

            if (content.style.maxHeight) {
                content.style.maxHeight = null;
                content.classList.remove("show");
            } else {
                content.style.maxHeight = content.scrollHeight + "px";
                content.classList.add("show");
            }

            setTimeout(() => {
                if (expanded && expandBox) {
                    expandBox.style.maxHeight = expandBox.scrollHeight + "px";
                }
            }, 300);
        });
    });
}

