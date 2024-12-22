'use client';

import Link from 'next/link';

const Sidebar = () => {
  return (
    <div className="w-64 h-screen bg-sidebar-bg text-sidebar-text flex flex-col p-4">
      <h1 className="text-xl font-bold mb-6">BuzzerWolf</h1>
      <nav className="flex-grow">
        <ul className="space-y-4">
          <li>
            <Link href="/home" className="block px-4 py-2 rounded hover:bg-sidebar-hover">
              Home
            </Link>
          </li>
          <li>
            <Link href="/leagues" className="block px-4 py-2 rounded hover:bg-sidebar-hover">
              Leagues
            </Link>
          </li>
          <li>
            <Link href="/logout" className="block px-4 py-2 rounded hover:bg-sidebar-hover">
              Log Out
            </Link>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;
