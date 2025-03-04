import React from "react";

const Sidebar: React.FC = () => {
  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-gray-800 text-white pt-10">
      <ul className="space-y-4">
        <div className="sidebar-text text-center font-bold text-xl">
        Onboarding veiledning  
        </div> 
        <li>
          <a href="/systemvalg" className="block px-4 py-2 text-lg hover:bg-gray-700 text-center">
            Systemvalg
          </a>
        </li>
        <li>
          <a href="/importvelger" className="block px-4 py-2 text-lg hover:bg-gray-700 text-center">
            Importvelger
          </a>
        </li>
        <li>
          <a href="upload" className="block px-4 py-2 text-lg hover:bg-gray-700 text-center">
            Last opp
          </a>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
