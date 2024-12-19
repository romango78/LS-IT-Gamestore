import { ReactComponent as TwitterLink } from './images/twitter-svgrepo-com.svg'
import { ReactComponent as FacebookLink } from './images/facebook-svgrepo-com.svg'
import { ReactComponent as MailLink } from './images/email-svgrepo-com.svg'
import { ReactComponent as InstagramLink } from './images/instagram-logo-svgrepo-com.svg'
import { ReactComponent as GooglePlusLink } from './images/google-plus-logo-symbol-svgrepo-com.svg'

export const mainMenuItems = [
  { id: "games-menu-item", title: "Games", link: "/games" },
  { id: "genres-menu-item", title: "Genres", link: "/genres" },
  { id: "publishers-menu-item", title: "Publishers", link: "/publishers" },
  { id: "orders-menu-item", title: "Orders", link: "/orders" },
  { id: "cart-menu-item", title: "Cart", link: "/cart" }
];

export const siteLinksMenuItems = [
  { id: "games-sitelink-item", title: "Games", link: "/games" },
  { id: "genres-sitelink-item", title: "Genres", link: "/genres" },
  { id: "publishers-sitelink-item", title: "Publishers", link: "/publishers" },
  { id: "orders-sitelink-item", title: "Orders", link: "/orders" },
  { id: "cart-sitelink-item", title: "My Bucket", link: "/cart" }
];

export const companyInfoMenuItems = [
  { id: "about-companyinfo-item", title: "About", link: "#" },
  { id: "award-companyinfo-item", title: "Award", link: "#" },
  { id: "reviews-companyinfo-item", title: "Reviews", link: "#" },
  { id: "testimonials-companyinfo-item", title: "Testimonials", link: "#" },
  { id: "contact-companyinfo-item", title: "Contact", link: "#" }
];

export const socialMediaMenuItems = [
  { id: "twitter-socialmedia-item", img: TwitterLink, link: "#" },
  { id: "facebook-socialmedia-item", img: FacebookLink, link: "#" },
  { id: "mail-socialmedia-item", img: MailLink, link: "#" },
  { id: "instagram-socialmedia-item", img: InstagramLink, link: "#" },
  { id: "googleplus-socialmedia-item", img: GooglePlusLink, link: "#" }
];