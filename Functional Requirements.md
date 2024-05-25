# Functional Requirements

## 1. **User Authentication**
- **FR1.1:** The website must support admin login with secure authentication.

## 2. **Blog Management**
- **FR2.1:** Admin must be able to create blogs with a title, content, embedded YouTube videos, and one or more images.
- **FR2.2:** The content editor must support rich text formatting.
- **FR2.3:** Admin must be able to categorize blogs.
- **FR2.4:** Admin must be able to view a list of their blogs.
- **FR2.5:** Blogs can be sorted and filtered by date created, view count, category, and alphabetically.
- **FR2.6:** Admin must be able to select and feature a set number of blogs on the homepage.

## 3. **Homepage**
- **FR3.1:** Display trending blogs based on view count.
- **FR3.2:** Featured blogs as selected by the admin are displayed below the trending blogs.
- **FR3.3:** List of blogs ordered by newest first.

## 4. **Blog Interaction**
- **FR4.1:** Users can view blogs in detail by clicking on them.
- **FR4.2:** Users can leave comments on blogs by entering their full name and email or connecting via Facebook.
- **FR4.3:** Users can share blogs on Facebook, Twitter, Threads, or by copying the blog URL.

## 5. **Navigation and Content Pages**
- **FR5.1:** Main blog page allows viewing all blogs with filters for date, popularity, and featured status.
- **FR5.2:** About Me page contains admin's bio, contact details, and social media links.
- **FR5.3:** Contact Me page enables users to send messages via a built-in mailer or view admin’s contact information.
- **FR5.4:** Footer includes social media links and a subscription field for newsletters.

## 6. **Newsletter Subscription**
- **FR6.1:** Users can subscribe to newsletters to receive email notifications for new blogs.

## 7. **Bot Protection**
- **FR7.1:** Implement CAPTCHA to protect the site from bots.

# Non-Functional Requirements

## 1. **Usability**
- **NFR1.1:** The website should be user-friendly with intuitive navigation.
- **NFR1.2:** Ensure accessibility for users with disabilities according to WCAG 2.1 standards.

## 2. **Performance**
- **NFR2.1:** Pages must load within 3 seconds to ensure a smooth user experience.
- **NFR2.2:** The website should handle at least 1,000 concurrent users without performance degradation.

## 3. **Security**
- **NFR3.1:** Implement SSL/TLS to secure user data in transit.
- **NFR3.2:** Ensure secure storage of user passwords using hashing and salting.

## 4. **Scalability**
- **NFR4.1:** The system must be scalable to accommodate a growing number of users and blog posts.

## 5. **Availability**
- **NFR5.1:** The website should be available with an uptime of 99.9%.

## 6. **Maintainability**
- **NFR6.1:** The code should be well-documented and maintainable.

## 7. **Design and Aesthetic**
- **NFR7.1:** The website should adhere to the color theme of black, white, and pink.
- **NFR7.2:** Focus on a light mode design; dark mode is optional and available at an additional cost.
